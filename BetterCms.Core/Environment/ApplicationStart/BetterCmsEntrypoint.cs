using System;
using System.Web;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Environment.ApplicationStart;
using BetterCms.Core.Exceptions;
using Common.Logging;

[assembly: PreApplicationStartMethod(typeof(BetterCmsEntrypoint), "PreApplicationStart")]

namespace BetterCms.Core.Environment.ApplicationStart
{
    /// <summary>
    /// Entry point to run BetterCMS preload  logic. 
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
                logger.Info("Starting BetterCMS...");
            }
            catch (Exception ex)
            {
                throw new CmsException("Logging is not working. A reason may be that Common.Logging section is not configured in web.config.", ex);
            }            

            if (!IsFullTrust)
            {
                string message = "Application should run under FullTrust .NET trust level.";
                logger.Fatal(message);
                
                throw new CmsException(message);
            }

            try
            {
                logger.Info("Creating BetterCMS context dependencies container...");
                ContextScopeProvider.RegisterTypes(BetterCmsContext.InitializeContainer());
            }
            catch (Exception ex)
            {
                string message = "Failed to create BetterCMS context dependencies container.";
                logger.Fatal(message, ex);

                throw new CmsException(message, ex);
            }

            try
            {
                logger.Info("Registering per web request lifetime manager module...");
                PerWebRequestLifetimeModule.DynamicModuleRegistration();
            }
            catch (Exception ex)
            {
                string message = "Failed to register per web request lifetime manager module.";
                logger.Fatal(message, ex);

                throw new CmsException(message, ex);
            }

            try
            {
                logger.Info("Load assemblies...");
                BetterCmsContext.LoadAssemblies();
            }
            catch (Exception ex)
            {
                string message = "Failed to load assemblies.";
                logger.Fatal(message, ex);

                throw new CmsException(message, ex);
            }
        }
    }
}