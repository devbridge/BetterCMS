using System.Web.Mvc;

using BetterCms.Module.Pages.Command.Content.GetPageContentOptions;
using BetterCms.Module.Pages.Command.Layout.GetSiteSettingsTemplates;
using BetterCms.Module.Pages.Command.Widget.DeleteWidget;
using BetterCms.Module.Pages.Command.Widget.GetHtmlContentWidgetForEdit;
using BetterCms.Module.Pages.Command.Widget.GetServerControlWidgetForEdit;
using BetterCms.Module.Pages.Command.Widget.GetSiteSettingsWidgets;
using BetterCms.Module.Pages.Command.Widget.SaveWidget;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Pages.Controllers
{
    public class TemplatesController : CmsControllerBase
    {
        /// <summary>
        /// Deletes template.
        /// </summary>
        /// <param name="id">The template id.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        public ActionResult DeleteTemplate(string id, string version)
        {
            var request = new DeleteWidgetRequest
            {
                WidgetId = id.ToGuidOrDefault(),
                Version = version.ToIntOrDefault()
            };
            if (GetCommand<DeleteWidgetCommand>().ExecuteCommand(request))
            {
                Messages.AddSuccess(PagesGlobalization.DeleteWidget_DeletedSuccessfully_Message);
                return Json(new WireJson
                {
                    Success = true
                });
            }

            return Json(new WireJson { Success = false });
        }
    

        /// <summary>
        /// Creates modal dialog for creating a new widget.
        /// </summary>
        /// <returns>
        /// ViewResult to render new widget modal dialog.
        /// </returns>
        [HttpGet]
        public ActionResult RegisterTemplate()
        {
            var model = GetCommand<GetServerControlWidgetForEditCommand>().ExecuteCommand(null);
            return PartialView("EditTemplate", model);
        }
/*
        /// <summary>
        /// Creates modal dialog for editing a widget.
        /// </summary>
        /// <returns>
        /// ViewResult to render editing widget modal dialog.
        /// </returns>
        [HttpGet]
        public ActionResult EditServerControlWidget(string id)
        {
            var model = GetCommand<GetServerControlWidgetForEditCommand>().ExecuteCommand(id.ToGuidOrDefault());
            return PartialView(model);
        }

        /// <summary>
        /// Validates and saves widget.
        /// </summary>
        /// <param name="model">The widget view model.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        public ActionResult EditServerControlWidget(ServerControlWidgetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveServerControlWidgetCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(PagesGlobalization.SaveWidget_CreatedSuccessfully_Message);
                    }
                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

        // TODO: remote it. use previewUrl.
        [HttpGet]
        public ActionResult PreviewHtmlContentWidget(string contentId)
        {
            HtmlContentWidgetViewModel model = GetCommand<GetHtmlContentWidgetForEditCommand>().ExecuteCommand(contentId.ToGuidOrDefault());
            return View(model);
        }

        // TODO: remote it. use previewUrl.
        [HttpGet]
        public ActionResult PreviewWidget(string widgetId)
        {
            ServerControlWidgetViewModel model = GetCommand<GetServerControlWidgetForEditCommand>().ExecuteCommand(widgetId.ToGuidOrDefault());
            return View(model);
        }*/

        /// <summary>
        /// Renders a widgets list for the site settings dialog.
        /// </summary>
        /// <returns>Rendered widgets list.</returns>
        public ActionResult Templates(SearchableGridOptions request)
        {
            var model = GetCommand<GetSiteSettingsTemplatesCommand>().ExecuteCommand(request);
            
            return View(model);
        }
    }
}