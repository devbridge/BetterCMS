using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Web;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Root.Mvc
{    
    /// <summary>
    /// Custom controller base.
    /// </summary>
    public abstract class CmsControllerBase : Controller, ICommandContext
    {
        /// <summary>
        /// View data key to store messages.
        /// </summary>
        private const string UserMessagesViewDataKey = "_UserMessages";

        private HttpContextTool httpContextTool;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsControllerBase" /> class.
        /// </summary>
        protected CmsControllerBase()
        {     
            HtmlHelper.ClientValidationEnabled = true;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;            

            RenderViewDelegate = (viewName, model, enableFormContext) =>
                {
                    if (string.IsNullOrEmpty(viewName))
                    {
                        viewName = ControllerContext.RouteData.GetRequiredString("action");
                    }

                    ViewData.Model = model;

                    using (var sw = new StringWriter())
                    {
                        var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                        if (enableFormContext && viewContext.FormContext == null)
                        {
                            viewContext.FormContext = new FormContext();
                        }

                        viewResult.View.Render(viewContext, sw);
                        viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                        return sw.GetStringBuilder().ToString();
                    }
                };
        }

        public virtual ICommandResolver CommandResolver { get; set; }

        /// <summary>
        /// Gets or sets the HTTP context helper tool.
        /// </summary>
        /// <value>
        /// The HTTP context helper tool.
        /// </value>
        public HttpContextTool Http
        {
            get
            {
                if (httpContextTool == null && HttpContext != null)
                {
                    httpContextTool = new HttpContextTool(HttpContext);
                }
                return httpContextTool;
            }
            set { httpContextTool = value; }
        }

        /// <summary>
        /// Delegate to render the partial view as raw html.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <param name="enableFormContext">if set to <c>true</c> [enable form context].</param>
        /// <returns>Rendered raw html.</returns>
        public delegate string RenderViewMethod(string viewName, object model, bool enableFormContext = false);

        /// <summary>
        /// Gets or sets the render view delegate.
        /// </summary>
        /// <value>
        /// The render view delegate.
        /// </value>
        public RenderViewMethod RenderViewDelegate { get; set; }

        /// <summary>
        /// Gets the user messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public virtual IMessagesIndicator Messages
        {
            get
            {
                var userMessages = ViewData[UserMessagesViewDataKey] as UserMessages;
               
                if (userMessages == null)
                {
                    userMessages = new UserMessages();
                    ViewData[UserMessagesViewDataKey] = userMessages;
                }

                return userMessages;
            }
        }

        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.ViewResult" /> object using the view name and master-page name that renders a view to the response.
        /// </summary>
        /// <param name="model">View model object.</param>
        /// <param name="contentType">Response content type.</param>
        /// <returns>
        /// The view result.
        /// </returns>
        [NonAction]
        public virtual ActionResult View(object model, string contentType)
        {
            Response.ContentType = contentType;

            return View(model);
        }

        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.JsonResult" /> object that serializes the specified object to JavaScript Object Notation (JSON) format.
        /// </summary>
        /// <param name="data">The JavaScript object graph to serialize.</param>
        /// <param name="behavior">Specifies whether HTTP GET requests from the client are allowed.</param>
        /// <returns>
        /// The JSON result object that serializes the specified object to JSON format.
        /// </returns>
        [NonAction]
        public virtual JsonResult Json(WireJson data, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            // TODO: create wirejson result.
            List<string> messages = data.Messages != null
                                        ? data.Messages.ToList()
                                        : new List<string>();

            messages.AddRange(data.Success ? Messages.Success : Messages.Error);
            data.Messages = messages.ToArray();

            return base.Json(data, behavior);
        }

        /// <summary>
        /// Renders the partial view.
        /// </summary>
        /// <param name="model">The model.</param>        
        /// <param name="enableFormContext">if set to <c>true</c> [enable form context].</param>
        /// <returns> Returns view rendered into String  </returns>
        [NonAction]
        public virtual string RenderView(object model, bool enableFormContext = false)
        {
            return RenderViewDelegate(null, model, enableFormContext);
        }

        /// <summary>
        /// Renders the partial view as raw html.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <param name="enableFormContext">if set to <c>true</c> [enable form context].</param>
        /// <returns>
        /// Returns rendered view raw html.
        /// </returns>
        [NonAction]
        public virtual string RenderView(string viewName, object model, bool enableFormContext = false)
        {
            return RenderViewDelegate(viewName, model, enableFormContext);
        }

        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!ModelState.IsValid)
            {
                var modelStateErrors = from item in ModelState.Values
                                       from error in item.Errors
                                       select error.ErrorMessage;

                Messages.AddError(modelStateErrors.ToArray());
            }

            base.OnActionExecuting(filterContext);
        }           
    
        protected TCommand GetCommand<TCommand>() where TCommand : ICommandBase
        {
            return CommandResolver.ResolveCommand<TCommand>(this);
        }
    }
}
