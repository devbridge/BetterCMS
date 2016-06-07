// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BetterCmsEntrypoint.cs" company="Devbridge Group LLC">
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
using System;

using BetterCms.Core.Environment.ApplicationStart;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Security;

using Common.Logging;

using BetterModules.Core.Dependencies;
using BetterModules.Core.Exceptions;
using BetterModules.Core.Web.Environment.Application;

[assembly: WebApplicationPreStart(typeof(BetterCmsEntrypoint), "PreApplicationStart", Order = 50)]

namespace BetterCms.Core.Environment.ApplicationStart
{
    /// <summary>
    /// Entry point to run Better CMS preload  logic. 
    /// </summary>
    public static class BetterCmsEntrypoint
    {
        /// <summary>
        /// Gets a value indicating whether this application is in full trust.
        /// </summary>
        /// <value>
        /// <c>true</c> if application is in full trust; otherwise, <c>false</c>.
        /// </value>
        internal static bool IsFullTrust
        {
            get { return AppDomain.CurrentDomain.IsHomogenous && AppDomain.CurrentDomain.IsFullyTrusted; }
        }

        /// <summary>
        /// Method to run logic before application start (as PreApplicationStartMethod). Do not run this method from your code.
        /// </summary>        
        public static void PreApplicationStart()
        {
            ILog logger;

            try
            {
                logger = LogManager.GetLogger(typeof(BetterCmsEntrypoint));
                logger.Info("Starting Better CMS...");
            }
            catch (Exception ex)
            {
                throw new CoreException("Logging is not working. A reason may be that Common.Logging section is not configured in web.config.", ex);
            }

            if (!IsFullTrust)
            {
                string message = "Application should run under FullTrust .NET trust level.";
                logger.Fatal(message);
                
                throw new CmsException(message);
            }

            try
            {
                logger.Info("Creating Better CMS context dependencies container...");
                ContextScopeProvider.RegisterTypes(CmsContext.InitializeContainer());
            }
            catch (Exception ex)
            {
                string message = "Failed to create Better CMS context dependencies container.";
                logger.Fatal(message, ex);

                throw new CmsException(message, ex);
            }

            try
            {
                logger.Info("Registering forms authentication redirect suppress module...");
                SuppressFormsAuthenticationRedirectModule.DynamicModuleRegistration();
            }
            catch (Exception ex)
            {
                string message = "Failed to register forms authentication redirect suppress module.";
                logger.Fatal(message, ex);

                throw new CmsException(message, ex);
            }
        }
    }
}