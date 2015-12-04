// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetChildContentOptionsCommand.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
//                    var childContent = AddFetches(draftQuery).ToList().FirstOrDefault();
                    var childContent = draftQuery.FirstOrDefault();
//                    var childContent = GetChildContent(request);

                    // If draft not found, load content
                    if (childContent == null)
                    {
                        var query = Repository.AsQueryable<ChildContent>().Where(
                                    f => f.Parent.Id == request.ContentId && f.AssignmentIdentifier == request.AssignmentIdentifier && !f.IsDeleted && !f.Child.IsDeleted);

                        childContent = query.First();
                        FetchCollections(childContent);
                    }
                    else
                    {
                        FetchCollections(childContent);
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
            // TODO: fix this !!! This has critical performance impact
            return query
                .Fetch(f => f.Child)
                .ThenFetchMany(f => f.ContentOptions)
                .ThenFetch(f => f.CustomOption)

                .FetchMany(f => f.Options)
                .ThenFetch(f => f.CustomOption)

                .Fetch(f => f.Child)
                .ThenFetchMany(f => f.History)
                .ThenFetchMany(f => f.ContentOptions)
                .ThenFetch(f => f.CustomOption);
        }
        private ChildContent GetChildContent(GetChildContentOptionsCommandRequest request)
        {
//            var draftQuery = Repository.AsQueryable<ChildContent>()
//                .Where(f => f.Parent.Original.Id == request.ContentId
//                    && !f.Parent.Original.IsDeleted
//                    && f.Parent.Original.Status == ContentStatus.Published
//                    && f.Parent.Status == ContentStatus.Draft
//                    && f.AssignmentIdentifier == request.AssignmentIdentifier
//                    && !f.IsDeleted
//                    && !f.Child.IsDeleted);
//            var childContent = AddFetches(draftQuery).ToList().FirstOrDefault();
            var query = Repository.AsQueryable<ChildContent>()

                .Where(f => f.Parent.Original.Id == request.ContentId
                    && !f.Parent.Original.IsDeleted
                    && f.Parent.Original.Status == ContentStatus.Published
                    && f.Parent.Status == ContentStatus.Draft
                    && f.AssignmentIdentifier == request.AssignmentIdentifier
                    && !f.IsDeleted
                    && !f.Child.IsDeleted)
                .Fetch(x => x.Parent)
                .ThenFetch(x => x.Original);
            var childContent = query.ToList().FirstOrDefault();

            if (childContent != null)
            {

            }
            return childContent;
        }

        private void FetchCollections(ChildContent childContent)
        {
            var childQuery = Repository.AsQueryable<Root.Models.Content>().Where(x => x.Id == childContent.Child.Id).ToFuture();
            var historyQuery = Repository.AsQueryable<Root.Models.Content>().Where(x => x.Original.Id == childContent.Child.Id).ToFuture();

            childContent.Child = childQuery.First();
            childContent.Child.History = historyQuery as IList<Root.Models.Content> ?? historyQuery.ToList();

            var historyIds = childContent.Child.History.Select(x => x.Id).ToList();
            var contentOptionsQuery =
                Repository.AsQueryable<ContentOption>()
                    .Where(x => x.Content.Id == childContent.Child.Id)
                    .FetchMany(x => x.Translations)
                    .Fetch(x => x.CustomOption)
                    .ToFuture();

            var childContentOptionsQuery = Repository.AsQueryable<ChildContentOption>().Where(x => x.ChildContent.Id == childContent.Id).Fetch(x => x.CustomOption).FetchMany(x => x.Translations).ToFuture();
            var historyContentOptionsQuery = Repository.AsQueryable<ContentOption>().Where(x => historyIds.Contains(x.Id)).Fetch(x => x.CustomOption).FetchMany(x => x.Translations).ToFuture();
            childContent.Child.ContentOptions = contentOptionsQuery.ToList();
            childContent.Options = childContentOptionsQuery as IList<ChildContentOption> ?? childContentOptionsQuery.ToList();
            if (childContent.Child.History.Any())
            {
                var historyContentOptions = historyContentOptionsQuery as IList<ContentOption> ?? historyContentOptionsQuery.ToList();
                foreach (var content in childContent.Child.History)
                {
                    content.ContentOptions = historyContentOptions.Where(x => x.Content.Id == content.Id).ToList();
                }
            }
        }
    }
}