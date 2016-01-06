using System;
using System.Web.Mvc;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Binders;
using BetterCms.Core.Security;

using BetterCms.Module.Pages.Command.Widget.DeleteWidget;
using BetterCms.Module.Pages.Command.Widget.GetHtmlContentWidgetForEdit;
using BetterCms.Module.Pages.Command.Widget.GetServerControlWidgetForEdit;
using BetterCms.Module.Pages.Command.Widget.GetSiteSettingsWidgets;
using BetterCms.Module.Pages.Command.Widget.GetWidgetCategory;
using BetterCms.Module.Pages.Command.Widget.GetWidgetUsages;
using BetterCms.Module.Pages.Command.Widget.PreviewWidget;
using BetterCms.Module.Pages.Command.Widget.SaveWidget;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.Widgets;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using BetterModules.Core.Web.Models;

using Common.Logging;

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
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
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
        /// <param name="widgetId">The id.</param>
        /// <returns>
        /// ViewResult to render a dialog for the html content widget editing.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult EditHtmlContentWidget(string widgetId)
        {
            var model = GetCommand<GetHtmlContentWidgetForEditCommand>().ExecuteCommand(widgetId.ToGuidOrDefault());
            var view = RenderView("EditHtmlContentWidget", model);
            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates and saves html content widget.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult EditHtmlContentWidget([ModelBinder(typeof(JSONDataBinder))] SaveWidgetCommandRequest<EditHtmlContentWidgetViewModel> request)
        {
            ValidateModelExplicitly(request.Content);

            try
            {
                if (ModelState.IsValid)
                {
                    var response = GetCommand<SaveHtmlContentWidgetCommand>().ExecuteCommand(request);
                    if (response != null)
                    {
                        if (request.Content.Id.HasDefaultValue())
                        {
                            Messages.AddSuccess(PagesGlobalization.SaveWidget_CreatedSuccessfully_Message);
                        }

                        return Json(new WireJson { Success = true, Data = response });
                    }
                }

                return Json(new WireJson { Success = false });
            }
            catch (ConfirmationRequestException exc)
            {
                return Json(new WireJson { Success = false, Data = new { ConfirmationMessage = exc.Resource() } });
            }
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
        /// <param name="request">The request.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult EditServerControlWidget([ModelBinder(typeof(JSONDataBinder))] SaveWidgetCommandRequest<EditServerControlWidgetViewModel> request)
        {
            ValidateModelExplicitly(request.Content);

            if (ModelState.IsValid)
            {
                ViewEngineResult viewEngineResult = null;
                try
                {
                    viewEngineResult = ViewEngines.Engines.FindView(ControllerContext, request.Content.Url, null);
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("Failed to get the view for server widget by url '{0}'.", request.Content.Url), ex);
                }
                if (viewEngineResult == null || viewEngineResult.View == null)
                {
                    Messages.AddError(string.Format(PagesGlobalization.SaveWidget_VirtualPathNotExists_Message, request.Content.Url));
                    return Json(new WireJson { Success = false });
                }

                var response = GetCommand<SaveServerControlWidgetCommand>().ExecuteCommand(request);
                if (response != null)
                {
                    if (request.Content.Id.HasDefaultValue())
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
        public ActionResult Widgets(WidgetsFilter request)
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

        /// <summary>
        /// Creates select widget modal dialog for given page.
        /// </summary>
        /// <returns>
        /// ViewResult to render select widget content modal dialog.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult SelectWidget(GetRecentWidgetAndWidgetCategoryRequest request)
        {
            var model = GetCommand<GetRecentWidgetAndWidgetCategoryCommand>().ExecuteCommand(request);
            var view = model != null ? RenderView("SelectWidget", model) : string.Empty;

            var result = ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
// TODO: very large JSON ~2.1MB on test environment!!!
//            var scriptSerializer = new JavaScriptSerializer();
//            if (result.MaxJsonLength.HasValue)
//            {
//                scriptSerializer.MaxJsonLength = result.MaxJsonLength.Value;
//            }
//            var jsonString = scriptSerializer.Serialize(result.Data);

            return result;
        }

        /// <summary>
        /// Returns the list of widget usages and template for rendering the results.
        /// </summary>
        /// <param name="widgetId">The widget identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// JSON result with template and list of widget usages
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult WidgetUsages(string widgetId, SearchableGridOptions options)
        {
            options.SetDefaultPaging();

            var request = new GetWidgetUsagesCommandRequest
                          {
                              Options = options,
                              WidgetId = widgetId.ToGuidOrDefault()
                          };

            var model = GetCommand<GetWidgetUsagesCommand>().ExecuteCommand(request);
            var view = RenderView("Partial/WidgetUsagesTemplate", null);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }
    }
}
