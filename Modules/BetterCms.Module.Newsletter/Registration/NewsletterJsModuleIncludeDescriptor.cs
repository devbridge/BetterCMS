// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsletterJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Newsletter.Content.Resources;
using BetterCms.Module.Newsletter.Controllers;

namespace BetterCms.Module.Newsletter.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class NewsletterJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsletterJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public NewsletterJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.newsletter")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<SubscriberController>(this, "loadSiteSettingsSubscribersUrl", c => c.ListTemplate()),
                    new JavaScriptModuleLinkTo<SubscriberController>(this, "loadSubscribersUrl", c => c.SubscribersList(null)),
                    new JavaScriptModuleLinkTo<SubscriberController>(this, "saveSubscriberUrl", c => c.SaveSubscriber(null)),
                    new JavaScriptModuleLinkTo<SubscriberController>(this, "deleteSubscriberUrl", c => c.DeleteSubscriber(null, null)),
                    new JavaScriptModuleLinkTo<SubscriberController>(this, "downoadCsvUrl", c => c.DownoadInCsv()),
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "deleteSubscriberDialogTitle", () => NewsletterGlobalization.DeleteSubscriber_Confirmation_Message), 
                };
        }
    }
}