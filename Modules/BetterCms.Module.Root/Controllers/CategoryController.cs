// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryController.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Root.Commands.Category.DeleteCategoryTree;
using BetterCms.Module.Root.Commands.Category.GetCategoryTree;
using BetterCms.Module.Root.Commands.Category.GetCategoryTreesList;
using BetterCms.Module.Root.Commands.Category.SaveCategoryTree;
using BetterCms.Module.Root.Commands.Category.SearchCategory;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels;
using BetterCms.Module.Root.ViewModels.Category;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Handles categories logic.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class CategoryController : CmsControllerBase
    {
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult CategoryTrees(CategoryTreesFilter request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetCategoryTreesListCommand>().ExecuteCommand(request);

            var view = RenderView("CategoryTrees", model);

            return ComboWireJson(model != null, view, new {}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult EditCategoryTree(string sitemapId)
        {
            var model = GetCommand<GetCategoryTreeCommand>().ExecuteCommand(sitemapId.ToGuidOrDefault());
            var success = model != null;
            var view = RenderView("CategoryTreeEdit", model);
            return ComboWireJson(success, view, model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult SaveCategoryTree(CategoryTreeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveCategoryTreeCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(RootGlobalization.CategoryTree_CategoryTreeCreatedSuccessfully_Message);
                    }

                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult DeleteCategoryTree(string id, string version)
        {
            var success =
                GetCommand<DeleteCategoryTreeCommand>().ExecuteCommand(new CategoryTreeViewModel
                                                                           {
                                                                               Id = id.ToGuidOrDefault(),
                                                                               Version = version.ToIntOrDefault()
                                                                           });

            if (success)
            {
                Messages.AddSuccess(RootGlobalization.CategoryTree_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }

        [HttpPost]
        public ActionResult SuggestCategories(CategorySuggestionViewModel model)
        {
            var suggestedTags = GetCommand<SearchCategoriesCommand>().ExecuteCommand(model);
            return Json(new { suggestions = suggestedTags });
        }
    }
}
