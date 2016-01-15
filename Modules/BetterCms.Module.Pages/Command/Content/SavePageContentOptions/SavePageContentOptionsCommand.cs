// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SavePageContentOptionsCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

using ContentEntity = BetterCms.Module.Root.Models.Content;

namespace BetterCms.Module.Pages.Command.Content.SavePageContentOptions
{
    public class SavePageContentOptionsCommand : CommandBase, ICommand<ContentOptionValuesViewModel, SavePageContentOptionsCommandResponse>
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
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public SavePageContentOptionsCommandResponse Execute(ContentOptionValuesViewModel model)
        {
            int version = 0;
            if (model != null && !model.OptionValuesContainerId.HasDefaultValue())
            {
                var pageContent = Repository.AsQueryable<PageContent>()
                              .Where(f => f.Id == model.OptionValuesContainerId && !f.IsDeleted && !f.Content.IsDeleted)
                              .FetchMany(f => f.Options)
                              .Fetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
                              .ToList()
                              .FirstOrDefault();

                if (pageContent != null)
                {
                    UnitOfWork.BeginTransaction();

                    var optionValues = pageContent.Options.Distinct();

                    pageContent.Options = OptionService.SaveOptionValues(model.OptionValues, optionValues, () => new PageContentOption { PageContent = pageContent });

                    UnitOfWork.Commit();

                    Events.PageEvents.Instance.OnPageContentConfigured(pageContent);

                    version = pageContent.Version;
                }                
            }

            return new SavePageContentOptionsCommandResponse() { PageContentVersion = version };
        }
    }
}