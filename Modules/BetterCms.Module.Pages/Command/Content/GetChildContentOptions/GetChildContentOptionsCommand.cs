using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

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
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the language service.
        /// </summary>
        /// <value>
        /// The language service.
        /// </value>
        public ILanguageService LanguageService { get; set; }

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
            var languagesFuture = CmsConfiguration.EnableMultilanguage ? LanguageService.GetLanguagesLookupValues() : new List<LookupKeyValue>();

            if (request.LoadOptions) { 
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
                        var content = GetDraftIfExists(childContent.Child);

                        model.OptionValuesContainerId = childContent.Id;
                        model.OptionValues = OptionService.GetMergedOptionValuesForEdit(content.ContentOptions, childContent.Options);
                        optionsLoaded = true;
                    }
                }
            
                if (!optionsLoaded)
                {
                    var content = Repository.AsQueryable<Root.Models.Content>()
                            .Where(c => c.Id == request.WidgetId)
                            .FetchMany(c => c.ContentOptions)
                            .ThenFetch(c => c.CustomOption)
                            .FetchMany(f => f.History).ThenFetchMany(f => f.ContentOptions).ThenFetch(f => f.CustomOption)
                            .ToList()
                            .FirstOne();

                    content = GetDraftIfExists(content);

                    model.OptionValues = OptionService.GetMergedOptionValuesForEdit(content.ContentOptions, null);
                }
            }

            model.CustomOptions = OptionService.GetCustomOptions();
            var languages = CmsConfiguration.EnableMultilanguage ? languagesFuture.ToList() : new List<LookupKeyValue>();
            model.Languages = languages;
            model.ShowLanguages = CmsConfiguration.EnableMultilanguage && languages.Any();

            return model;
        }

        private Root.Models.Content GetDraftIfExists(Root.Models.Content content)
        {
            if (content.Status != ContentStatus.Draft)
            {
                var draftContent = content.History.FirstOrDefault(c => c.Status == ContentStatus.Draft);
                if (draftContent != null)
                {
                    content = draftContent;
                }
            }

            return content;
        }

        private IQueryable<ChildContent> AddFetches(IQueryable<ChildContent> query)
        {
            return query
                .Fetch(f => f.Child).ThenFetchMany(f => f.ContentOptions).ThenFetch(f => f.CustomOption)
                .FetchMany(f => f.Options).ThenFetch(f => f.CustomOption)
                .Fetch(f => f.Child).ThenFetchMany(f => f.History).ThenFetchMany(f => f.ContentOptions).ThenFetch(f => f.CustomOption);
        }
    }
}