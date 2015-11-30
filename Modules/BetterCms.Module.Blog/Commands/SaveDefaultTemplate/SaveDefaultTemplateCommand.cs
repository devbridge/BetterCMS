// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveDefaultTemplateCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.SaveDefaultTemplate
{
    public class SaveDefaultTemplateCommand : CommandBase, ICommand<DefaultTemplateViewModel, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>True</c>, if save successful</returns>
        public bool Execute(DefaultTemplateViewModel request)
        {
            var option = Repository.AsQueryable<Option>().OrderByDescending(o => o.CreatedOn).FirstOrDefault(o => !o.IsDeleted);
            if (option == null)
            {
                option = new Option
                {
                    DefaultContentTextMode = ContentTextMode.Html
                };
            }

            if (!request.MasterPageId.HasDefaultValue())
            {
                option.DefaultMasterPage = Repository.AsProxy<Page>(request.MasterPageId);
                option.DefaultLayout = null;
            }
            else
            {
                option.DefaultLayout = Repository.AsProxy<Layout>(request.TemplateId);
                option.DefaultMasterPage = null;
            }

            Repository.Save(option);
            UnitOfWork.Commit();

            return true;
        }
    }
}