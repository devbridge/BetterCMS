// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscriberController.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Web.Mvc;

using BetterCms.Core.Security;

using BetterCms.Module.Newsletter.Command.DeleteSubscriber;
using BetterCms.Module.Newsletter.Command.GetAllSubscribersCsv;
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

        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult DownoadInCsv()
        {
            var stream = GetCommand<GetAllSubscribersStreamCsvCommand>().ExecuteCommand(null);
            return File(stream, "text/csv", "subscribers.csv");
        }

    }
}
