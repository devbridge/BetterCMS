using System.Web.Mvc;

using BetterCms.Core.Security;

using BetterCms.Module.Newsletter.Command.DeleteSubscriber;
using BetterCms.Module.Newsletter.Command.GetSubscriberList;
using BetterCms.Module.Newsletter.Command.SaveSubscriber;
using BetterCms.Module.Newsletter.Content.Resources;
using BetterCms.Module.Newsletter.ViewModels;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Newsletter.Controllers
{
    /// <summary>
    /// Newsletter subscribers controller.
    /// </summary>
    [ActionLinkArea(NewsletterModuleDescriptor.NewsletterAreaName)]
    public class SubscriberController : CmsControllerBase
    {
        /// <summary>
        /// Lists the template for displaying subscribers list.
        /// </summary>
        /// <returns>Json result.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult ListTemplate()
        {
            var view = RenderView("List", null);
            var request = new SearchableGridOptions();
            request.SetDefaultPaging();

            var subscribers = GetCommand<GetSubscriberListCommand>().ExecuteCommand(request);

            return ComboWireJson(subscribers != null, view, subscribers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lists the newsletter subscribers.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Json result.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SubscribersList(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetSubscriberListCommand>().ExecuteCommand(request);
            return WireJson(model != null, model);
        }

        /// <summary>
        /// Saves the newsletter subscriber.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SaveSubscriber(SubscriberViewModel model)
        {
            var success = false;
            SubscriberViewModel response = null;
            if (ModelState.IsValid)
            {
                response = GetCommand<SaveSubscriberCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(NewsletterGlobalization.CreateSubscriber_CreatedSuccessfully_Message);
                    }

                    success = true;
                }
            }

            return WireJson(success, response);
        }

        /// <summary>
        /// Saves the newsletter subscriber - public method.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        public ActionResult Subscribe(SubscriberViewModel model)
        {
            model.IgnoreUniqueSubscriberException = true;
            return SaveSubscriber(model);
        }

        /// <summary>
        /// Deletes the newsletter subscriber.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult DeleteSubscriber(string id, string version)
        {
            var request = new SubscriberViewModel { Id = id.ToGuidOrDefault(), Version = version.ToIntOrDefault() };
            var success = GetCommand<DeleteSubscriberCommand>().ExecuteCommand(request);
            if (success)
            {
                if (!request.Id.HasDefaultValue())
                {
                    Messages.AddSuccess(NewsletterGlobalization.DeleteSubscriber_DeletedSuccessfully_Message);
                }
            }

            return WireJson(success);
        }
    }
}
