using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Autofac;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services;
using BetterCms.Core.Web;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Helpers;

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
        /// Gets the security service.
        /// </summary>
        /// <value>
        /// The security service.
        /// </value>
        public ISecurityService SecurityService
        {
            get
            {
                var container = PerWebRequestContainerProvider.GetLifetimeScope(HttpContext);
                if (container != null && container.IsRegistered<ISecurityService>())
                {
                    return container.Resolve<ISecurityService>();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the principal of current command context.
        /// </summary>
        /// <value>
        /// The current command principal.
        /// </value>
        IPrincipal ICommandContext.Principal
        {
            get
            {
                return User;
            }
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
        /// Initializes a new instance of the <see cref="CmsControllerBase" /> class.
        /// </summary>
        protected CmsControllerBase()
        {     
            HtmlHelper.ClientValidationEnabled = true;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;

            RenderViewDelegate = this.RenderViewToString;
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
            List<string> messages = data.Messages != null
                                        ? data.Messages.ToList()
                                        : new List<string>();

            messages.AddRange(data.Success ? Messages.Success : Messages.Error);
            data.Messages = messages.ToArray();

            return base.Json(data, behavior);
        }

        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.JsonResult" /> object that serializes the specified object to JavaScript Object Notation (JSON) format.
        /// </summary>
        /// <param name="success">The request result.</param>
        /// <param name="data">The JavaScript object graph to serialize.</param>
        /// <param name="behavior">Specifies whether HTTP GET requests from the client are allowed.</param>
        /// <returns>
        /// The JSON result object that serializes the specified object to JSON format.
        /// </returns>
        [NonAction]
        public virtual JsonResult WireJson(bool success, object data = null, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            return Json(new WireJson { Success = success, Data = data }, behavior);
        }

        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.JsonResult" /> object that serializes the specified object and Html combined to JavaScript Object Notation (JSON) format.
        /// </summary>
        /// <param name="success">The request result.</param>
        /// <param name="html">The HTML.</param>
        /// <param name="data">The JavaScript object graph to serialize.</param>
        /// <param name="behavior">Specifies whether HTTP GET requests from the client are allowed.</param>
        /// <returns>
        /// The JSON result object that serializes the specified object to JSON format combined with Html.
        /// </returns>
        [NonAction]
        public virtual JsonResult ComboWireJson(bool success, string html, dynamic data, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            return Json(new ComboWireJson(success, html, data), behavior);
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
        /// Signs out user.
        /// </summary>
        [NonAction]
        protected virtual ActionResult SignOutUserIfAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (FormsAuthentication.IsEnabled)
                {
                    FormsAuthentication.SignOut();
                }

                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                HttpCookie roleCokie = Roles.Enabled ? Request.Cookies[Roles.CookieName] : null;

                if (authCookie != null)
                {
                    Response.Cookies.Add(
                        new HttpCookie(authCookie.Name)
                            {
                                Expires = DateTime.Now.AddDays(-10)
                            });
                }

                if (roleCokie != null)
                {
                    Response.Cookies.Add(
                        new HttpCookie(roleCokie.Name)
                            {
                                Expires = DateTime.Now.AddDays(-10)
                            });
                }
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UpdateModelStateErrors();

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Validates the model explicilty.
        /// </summary>
        /// <param name="model">The model.</param>
        protected virtual void ValidateModelExplicilty(object model)
        {
            if (!TryValidateModel(model))
            {
                UpdateModelStateErrors();
            }
        }

        /// <summary>
        /// Updates the model state errors.
        /// </summary>
        private void UpdateModelStateErrors()
        {
            if (!ModelState.IsValid)
            {
                var modelStateErrors = from item in ModelState.Values
                                       from error in item.Errors
                                       select error.ErrorMessage;

                Messages.AddError(modelStateErrors.ToArray());
            }
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <returns>Command instance</returns>
        protected TCommand GetCommand<TCommand>() where TCommand : ICommandBase
        {
            return CommandResolver.ResolveCommand<TCommand>(this);
        }

        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.JsonResult" /> object that serializes the specified object to JavaScript Object Notation (JSON) format using the content type, content encoding, and the JSON request behavior.
        /// </summary>
        /// <returns>
        /// The result object that serializes the specified object to JSON format.
        /// </returns>
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }
    }
}
