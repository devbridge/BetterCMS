using System;
using System.Linq;
using System.Web.Mvc;

using BetterCms.Module.Pages.Command.Content.GetPageContentOptions;
using BetterCms.Module.Pages.Command.Content.InsertContent;
using BetterCms.Module.Pages.Command.Content.SaveContent;
using BetterCms.Module.Pages.Command.Content.SavePageContentOptions;
using BetterCms.Module.Pages.Command.Content.SortPageContent;
using BetterCms.Module.Pages.Command.Content.DeletePageContent;
using BetterCms.Module.Pages.Command.Content.GetContent;
using BetterCms.Module.Pages.Command.Widget.GetWidgetCategory;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    public class ContentController : CmsControllerBase
    {
        /// <summary>
        /// Inserts content to given page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="contentId">The widget id.</param>
        /// <param name="regionId">The region id.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        public ActionResult InsertContentToPage(string pageId, string contentId, string regionId)
        {
            var request = new InsertContentToPageRequest
            {
                ContentId = contentId.ToGuidOrDefault(),
                PageId = pageId.ToGuidOrDefault(),
                RegionId = regionId.ToGuidOrDefault()
            };

            if (GetCommand<InsertContentToPageCommand>().ExecuteCommand(request))
            {
                return Json(new WireJson { Success = true });
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Creates a widget categories partial view for given search query.
        /// </summary>
        /// <param name="query">The widgets search query.</param>
        /// <returns>
        /// ViewResult to render a widget categories partial view.
        /// </returns>
        [HttpGet]
        public ActionResult Widgets(string query)
        {
            var request = new GetWidgetCategoryRequest { Filter = query };
            var model = GetCommand<GetWidgetCategoryCommand>().ExecuteCommand(request);

            return PartialView(model.WidgetCategories);
        }

        /// <summary>
        /// Creates widget categories partial view for given category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns>
        /// Html with rendered partial view.
        /// </returns>
        [HttpPost]
        public ActionResult WidgetCategory(string categoryId)
        {
            var result = GetCommand<GetWidgetCategoryCommand>().ExecuteCommand(new GetWidgetCategoryRequest
                                                                            {
                                                                                CategoryId = categoryId.ToGuidOrDefault(),
                                                                                Filter = null
                                                                            });
            var model = result.WidgetCategories.FirstOrDefault();

            return PartialView("Partial/WidgetCategory", model);
        }

        /// <summary>
        /// Creates add page content modal dialog for given page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="regionId">The region id.</param>
        /// <returns>
        /// ViewResult to render add page content modal dialog.
        /// </returns>
        [HttpGet]
        public ActionResult AddPageContent(string pageId, string regionId)
        {
            var viewModel = new PageContentViewModel
            {
                PageId = Guid.Parse(pageId),
                RegionId = Guid.Parse(regionId),
                LiveFrom = DateTime.Today
            };

            var request = new GetWidgetCategoryRequest();
            viewModel.WidgetCategories = GetCommand<GetWidgetCategoryCommand>().ExecuteCommand(request).WidgetCategories;            

            return View(viewModel);
        }

        /// <summary>
        /// Validates and saves page content.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Json with result status and redirect Url.
        /// </returns>
        [HttpPost]
        public ActionResult AddPageContent(PageContentViewModel model)
        {
            if (GetCommand<SaveContentCommand>().ExecuteCommand(model))
            {
                return Json(new WireJson { Success = true });
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Creates edit page content modal dialog for given page.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <returns>
        /// ViewResult to render edit page content modal dialog.
        /// </returns>
        [HttpGet]
        public ActionResult EditPageContent(string contentId)
        {
            var viewModel = GetCommand<GetContentCommand>().ExecuteCommand(contentId.ToGuidOrDefault());
            return View(viewModel);
        }

        /// <summary>
        /// Creates modal dialog for editing a page content options.
        /// </summary>
        /// <returns>
        /// ViewResult to render page content options modal dialog.
        /// </returns>
        [HttpGet]
        public ActionResult PageContentOptions(string pageContentId)
        {
            var model = GetCommand<GetPageContentOptionsCommand>().ExecuteCommand(pageContentId.ToGuidOrDefault());

            return View(model);
        }

        /// <summary>
        /// Saves page content options.
        /// </summary>
        /// <param name="model">The view model.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        public ActionResult PageContentOptions(PageContentOptionsViewModel model)
        {
            bool success = GetCommand<SavePageContentOptionsCommand>().ExecuteCommand(model);

            return Json(new WireJson { Success = success });
        }

        /// <summary>
        /// Deletes page content.
        /// </summary>
        /// <param name="pageContentId">Page content id.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        public ActionResult DeletePageContent(string pageContentId, string pageContentVersion, string ContentVersion)
        {
            var request = new DeletePageContentCommandRequest
                              {
                                  pageContentId = pageContentId.ToGuidOrDefault(),
                                  PageContentVersion = pageContentVersion.ToIntOrDefault(),
                                  ContentVersion = ContentVersion.ToIntOrDefault(),
                              };

            bool success = GetCommand<DeletePageContentCommand>().ExecuteCommand(request);

            return Json(new WireJson { Success = success });
        }

        /// <summary>
        /// Sorts page content.
        /// </summary>
        /// <param name="model">The view model.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        public ActionResult SortPageContent(PageContentSortViewModel model)
        {
            var response = GetCommand<SortPageContentCommand>().ExecuteCommand(model);
            return Json(new WireJson { Success = true, Data = response });
        }
    }
}
