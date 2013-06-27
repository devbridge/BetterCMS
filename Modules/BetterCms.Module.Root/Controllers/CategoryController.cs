using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Root.Commands.Category.DeleteCategory;
using BetterCms.Module.Root.Commands.Category.GetCategoryList;
using BetterCms.Module.Root.Commands.Category.SaveCategory;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.Category;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Handles categories logic.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class CategoryController : CmsControllerBase
    {       
        /// <summary>
        /// Renders a category list for the site settings dialog.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Rendered category list.
        /// </returns>
        public ActionResult Categories(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetCategoryListCommand>().ExecuteCommand(request);

            return View(model);
        }

        /// <summary>
        /// An action to save the category.
        /// </summary>
        /// <param name="category">The category data.</param>
        /// <returns>Json with status.</returns>
        [HttpPost]
        public ActionResult SaveCategory(CategoryItemViewModel category)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveCategoryCommand>().ExecuteCommand(category);
                if (response != null)
                {
                    if (category.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(RootGlobalization.CreateCategory_CreatedSuccessfully_Message);
                    }

                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// An action to delete a given category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>
        /// Json with status.
        /// </returns>
        [HttpPost]
        public ActionResult DeleteCategory(CategoryItemViewModel category)
        {
            bool success = GetCommand<DeleteCategoryCommand>().ExecuteCommand(                
                new DeleteCategoryCommandRequest
                    {
                        CategoryId = category.Id,
                        Version = category.Version
                    });

            if (success)
            {
                Messages.AddSuccess(RootGlobalization.DeleteCategory_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }
    }
}
