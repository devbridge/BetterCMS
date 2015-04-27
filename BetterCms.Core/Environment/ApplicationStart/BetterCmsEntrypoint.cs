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
                logger = LogManager.GetCurrentClassLogger();
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