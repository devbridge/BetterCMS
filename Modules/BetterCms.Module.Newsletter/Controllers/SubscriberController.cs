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

namespace BetterCms.Module.Newsletter.Controllers
{
    /// <summary>
    /// Newsletter subscribers controller.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    public class SubscriberController : CmsControllerBase
    {
        /// <summary>
        /// Lists the template for dispaying subscribers list.
        /// </summary>
        /// <returns>Json result.</returns>
        public ActionResult ListTemplate()
        {
            var view = RenderView("List", null);
            var subscribers = GetCommand<GetSubscriberListCommand>().ExecuteCommand(new SearchableGridOptions());

            return ComboWireJson(subscribers != null, view, subscribers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lists the newsletter subscribers.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Json result.</returns>
        public ActionResult SubscribersList(SearchableGridOptions request)
        {
            var model = GetCommand<GetSubscriberListCommand>().ExecuteCommand(request);
            return WireJson(model != null, model);
        }

        /// <summary>
        /// Saves the newsletter subscriber.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
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
        /// Deletes the newsletter subscriber.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
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
