// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPageForDeleteCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Page.GetPageForDelete
{
    /// <summary>
    /// Command for page delete confirmation.
    /// </summary>
    public class GetPageForDeleteCommand : CommandBase, ICommand<Guid, DeletePageViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Delete confirmation view model.</returns>
        public DeletePageViewModel Execute(Guid request)
        {
            var page = Repository
                .AsQueryable<PageProperties>(p => p.Id == request)
                .Fetch(p => p.PagesView)
                .FirstOne();
            string message = null;

            if (page.IsMasterPage && Repository.AsQueryable<MasterPage>(mp => mp.Master == page).Any())
            {
                message = PagesGlobalization.DeletePageCommand_MasterPageHasChildren_Message;
            }

            var model = new DeletePageViewModel
                {
                    PageId = page.Id,
                    Version = page.Version,
                    IsInSitemap = page.PagesView.IsInSitemap,
                    ValidationMessage = message
                };
            model.UpdateSitemap = model.IsInSitemap;
            return model;
        }
    }
}