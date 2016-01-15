// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPageContentOptionsCommand.cs" company="Devbridge Group LLC">
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
using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Core.Security;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using BetterModules.Core.Web.Mvc.Commands;

using FluentNHibernate.Testing.Values;

using NHibernate;
using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Content.GetPageContentOptions
{
    public class GetPageContentOptionsCommand : CommandBase, ICommand<Guid, ContentOptionValuesViewModel>
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
        /// Executes the specified request.
        /// </summary>
        /// <param name="pageContentId">The page content id.</param>
        /// <returns></returns>        
        public ContentOptionValuesViewModel Execute(Guid pageContentId)
        {
            var model = new ContentOptionValuesViewModel
            {
                OptionValuesContainerId = pageContentId
            };

            if (!pageContentId.HasDefaultValue())
            {
                var contentQuery = Repository.AsQueryable<PageContent>()
                    .Where(f => f.Id == pageContentId && !f.IsDeleted && !f.Content.IsDeleted);

                IEnumerable<AccessRule> accessRules = new List<AccessRule>();
                if (CmsConfiguration.Security.AccessControlEnabled)
                {
                    accessRules = contentQuery.SelectMany(t => t.Page.AccessRules).ToFuture();
                }

                var contentHistory = contentQuery.SelectMany(t => t.Content.History).ToFuture();
                var contentOptions = contentQuery.SelectMany(t => t.Content.ContentOptions).ToFuture();
                var pageOptions = contentQuery.SelectMany(t => t.Options).ToFuture();
                contentQuery = contentQuery.Fetch(t => t.Content).FetchMany(t=>t.Options).ThenFetch(t=>t.CustomOption);
                var pageContent = contentQuery.ToFuture().FirstOrDefault();

                if (pageContent != null)
                {
                    pageContent.Content.History = contentHistory.ToList();
                    pageContent.Content.ContentOptions = contentOptions.ToList();
                    pageContent.Options = pageOptions.ToList();
                    var contentToProject = pageContent.Content;
                    if (contentToProject.Status != ContentStatus.Draft)
                    {
                        var draftContent = contentToProject.History.FirstOrDefault(c => c.Status == ContentStatus.Draft);
                        if (draftContent != null)
                        {
                            contentToProject = draftContent;
                        }
                    }

                    model.OptionValues = OptionService.GetMergedOptionValuesForEdit(contentToProject.ContentOptions, pageContent.Options);
                    model.CustomOptions = OptionService.GetCustomOptions();

                    if (CmsConfiguration.Security.AccessControlEnabled)
                    {
                        SetIsReadOnly(model, accessRules.Cast<IAccessRule>().ToList());
                    }

                    if (CmsConfiguration.EnableMultilanguage)
                    {
                        var pageLanguage = Repository.AsQueryable<Root.Models.Page>().Where(p => p.Id == pageContent.Page.Id).Select(x => x.Language).FirstOrDefault();
                        if (pageLanguage != null)
                        {
                            SetTranslatedDefaultOptionValues(model.OptionValues, pageLanguage.Id);
                        }
                    }
                }
            }

            return model;
        }

        private void SetTranslatedDefaultOptionValues(IList<OptionValueEditViewModel> optionValues, Guid id)
        {
            foreach (var optionValueEditViewModel in optionValues)
            {
                if (optionValueEditViewModel.Translations != null)
                {
                    var translation = optionValueEditViewModel.Translations.FirstOrDefault(x => x.LanguageId == id.ToString());
                    if (translation != null)
                    {
                        optionValueEditViewModel.OptionDefaultValue = translation.OptionValue;
                        if (optionValueEditViewModel.UseDefaultValue)
                        {
                            optionValueEditViewModel.OptionValue = translation.OptionValue;
                        }
                    }
                }
            }
        }
    }
}