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
