using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Content.GetChildContentOptions
{
    public class GetChildContentOptionsCommand : CommandBase, ICommand<GetChildContentOptionsCommandRequest, ContentOptionValuesViewModel>
    {
        /// <summary>
        /// Gets or sets the option service.
        /// </summary>
        /// <value>
        /// The option service.
        /// </value>
        public IOptionService OptionService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public ContentOptionValuesViewModel Execute(GetChildContentOptionsCommandRequest request)
        {
            if (request.WidgetId.HasDefaultValue() && request.AssignmentIdentifier.HasDefaultValue())
            {
                var message = "WidgetId and AssignmentId should be set";
                throw new ValidationException(() => message, message);
            }

            var model = new ContentOptionValuesViewModel();
            var optionsLoaded = false;

            if (!request.AssignmentIdentifier.HasDefaultValue())
            {
                // Try get draft
                var draftQuery = Repository.AsQueryable<ChildContent>()
                    .Where(f => f.Parent.Original.Id == request.ContentId 
                        && !f.Parent.Original.IsDeleted
                        && f.Parent.Original.Status == ContentStatus.Published
                        && f.Parent.Status == ContentStatus.Draft
                        && f.AssignmentIdentifier == request.AssignmentIdentifier
                        && !f.IsDeleted
                        && !f.Child.IsDeleted);
                var childContent = AddFetches(draftQuery).ToList().FirstOrDefault();

                // If draft not found, load content
                if (childContent == null) {
                    var query = Repository.AsQueryable<ChildContent>()
                        .Where(f => f.Parent.Id == request.ContentId 
                            && f.AssignmentIdentifier == request.AssignmentIdentifier 
                            && !f.IsDeleted 
                            && !f.Child.IsDeleted);

                    childContent = AddFetches(query).ToList().FirstOrDefault();
                }

                if (childContent != null)
                {
                    model.OptionValuesContainerId = childContent.Id;
                    model.OptionValues = OptionService.GetMergedOptionValuesForEdit(childContent.Child.ContentOptions, childContent.Options);
                    optionsLoaded = true;
                }
            }
            
            if (!optionsLoaded)
            {
                var content = Repository.AsQueryable<Root.Models.Content>()
                        .Where(c => c.Id == request.WidgetId)
                        .FetchMany(c => c.ContentOptions)
                        .ThenFetch(c => c.CustomOption)
                        .ToList()
                        .FirstOne();

                model.OptionValues = OptionService.GetMergedOptionValuesForEdit(content.ContentOptions, null);
            }

            model.CustomOptions = OptionService.GetCustomOptions();

            return model;
        }

        private IQueryable<ChildContent> AddFetches(IQueryable<ChildContent> query)
        {
            return query.Fetch(f => f.Child)
                .ThenFetchMany(f => f.ContentOptions)
                .ThenFetch(f => f.CustomOption)
                .FetchMany(f => f.Options)
                .ThenFetch(f => f.CustomOption);
        }
    }
}