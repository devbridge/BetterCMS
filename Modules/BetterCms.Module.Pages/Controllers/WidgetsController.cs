using System.Linq;
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Pages.Command.History.GetContentVersion;
using BetterCms.Module.Pages.Command.Widget.DeleteWidget;
using BetterCms.Module.Pages.Command.Widget.GetHtmlContentWidgetForEdit;
using BetterCms.Module.Pages.Command.Widget.GetServerControlWidgetForEdit;
using BetterCms.Module.Pages.Command.Widget.GetSiteSettingsWidgets;
using BetterCms.Module.Pages.Command.Widget.PreviewWidget;
using BetterCms.Module.Pages.Command.Widget.SaveWidget;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    /// <summary>
    /// Widget management.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(PagesModuleDescriptor.PagesAreaName)]
    public class WidgetsController : CmsControllerBase
    {                    
        /// <summary>
        /// Deletes widget.
        /// </summary>
        /// <param name="id">The widget id.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult DeleteWidget(string id, string version)
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
        /// Creates modal dialog for creating a new html content widget.
        /// </summary>
        /// <returns>
        /// ViewResult to render a dialog for the new html content widget creation.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult CreateHtmlContentWidget()
        {
            var model = GetCommand<GetHtmlContentWidgetForEditCommand>().ExecuteCommand(null);
            model.EnableCustomCSS = true;
            model.EnableCustomHtml = true;
            model.EnableCustomJS = true;
            model.EditInSourceMode = true;

            var view = RenderView("EditHtmlContentWidget", model);

            return ComboWireJson(true, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creates modal dialog for editing an html content widget.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// ViewResult to render a dialog for the html content widget editing.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult EditHtmlContentWidget(string id)
        {
            var model = GetCommand<GetHtmlContentWidgetForEditCommand>().ExecuteCommand(id.ToGuidOrDefault());
            var view = RenderView("EditHtmlContentWidget", model);
            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates and saves html content widget.
        /// </summary>
        /// <param name="model">The html content widget view model.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult EditHtmlContentWidget(EditHtmlContentWidgetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveHtmlContentWidgetCommand>().ExecuteCommand(model);
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

        /// <summary>
        /// Creates modal dialog for creating a new widget.
        /// </summary>
        /// <returns>
        /// ViewResult to render new widget modal dialog.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult CreateServerControlWidget()
        {
            var model = GetCommand<GetServerControlWidgetForEditCommand>().ExecuteCommand(null);
            var view = RenderView("EditServerControlWidget", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creates modal dialog for editing a widget.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// ViewResult to render editing widget modal dialog.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult EditServerControlWidget(string id)
        {
            var model = GetCommand<GetServerControlWidgetForEditCommand>().ExecuteCommand(id.ToGuidOrDefault());
            var view = RenderView("EditServerControlWidget", model);
            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates and saves widget.
        /// </summary>
        /// <param name="model">The widget view model.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult EditServerControlWidget(EditServerControlWidgetViewModel model)
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

        /// <summary>
        /// Renders widget preview.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        /// <param name="enableJavaScript">if set to <c>true</c> enable java script.</param>
        /// <returns>
        /// View with widget preview
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration, RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent)]
        public ActionResult PreviewWidget(string widgetId, bool enableJavaScript)
        {
            var request = new PreviewWidgetCommandRequest { WidgetId = widgetId.ToGuidOrDefault(), IsJavaScriptEnabled = enableJavaScript };
            var model = GetCommand<PreviewWidgetCommand>().ExecuteCommand(request);

            return View(PagesConstants.ContentVersionPreviewTemplate, model);
        }

        /// <summary>
        /// Renders a widgets list for the site settings dialog.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Rendered widgets list.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult Widgets(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetSiteSettingsWidgetsCommand>().ExecuteCommand(request);

            // TODO: add servercontrolwidgetvalidation command and check if server controls exists in the server.
            /*if (model.ValidationMessages != null && model.ValidationMessages.Count > 0)
            {
                Messages.AddWarn(model.ValidationMessages.ToArray());
            }
            */

            return View(model);
        }
    }
}
