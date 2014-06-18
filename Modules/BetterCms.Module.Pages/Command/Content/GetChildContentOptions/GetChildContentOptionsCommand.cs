using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

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
            var model = new ContentOptionValuesViewModel();

            var childContent = Repository.AsQueryable<ChildContent>()
                .Where(f => f.Parent.Id == request.ContentId 
                    && f.AssignmentIdentifier == request.AssignmentIdentifier 
                    && !f.IsDeleted && !f.Child.IsDeleted)
                .Fetch(f => f.Child).ThenFetchMany(f => f.ContentOptions).ThenFetch(f => f.CustomOption)
                .FetchMany(f => f.Options).ThenFetch(f => f.CustomOption)
                .ToList()
                .FirstOne();

            if (childContent != null)
            {
                model.OptionValuesContainerId = childContent.Id;
                model.OptionValues = OptionService.GetMergedOptionValuesForEdit(childContent.Child.ContentOptions, childContent.Options);
                model.CustomOptions = OptionService.GetCustomOptions();
            }

            return model;
        }        
    }
}