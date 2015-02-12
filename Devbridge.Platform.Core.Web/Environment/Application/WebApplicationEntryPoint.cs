using System;

using Common.Logging;

using Devbridge.Platform.Core.Dependencies;
using Devbridge.Platform.Core.Exceptions;
using Devbridge.Platform.Core.Web.Dependencies;
using Devbridge.Platform.Core.Web.Environment.Application;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApplicationEntryPoint), "PreApplicationStart", Order = 100)]

namespace Devbridge.Platform.Core.Web.Environment.Application
{
    /// <summary>
    /// Entry point to run web application preload  logic. 
    /// </summary>
    public class WebApplicationEntryPoint
    {
        private static bool isStarted;

        /// <summary>
        /// Method to run logic before application start (as PreApplicationStartMethod). Do not run this method from your code.
        /// </summary>        
        internal static void PreApplicationStart()
        {
            if (isStarted)
            {
                return;
            }

            ILog logger;

            try
            {
                logger = LogManager.GetCurrentClassLogger();
                logger.Info("Starting Web Application...");
            }
            catch (Exception ex)
            {
                throw new PlatformException("Logging is not working. A reason may be that Common.Logging section is not configured in web.config.", ex);
            } 

            try
            {
                logger.Info("Creating Web Application context dependencies container...");
                ContextScopeProvider.RegisterTypes(WebApplicationContext.InitializeContainer());
            }
            catch (Exception ex)
            {
                string message = "Failed to create Web Application context dependencies container.";
                logger.Fatal(message, ex);

                throw new PlatformException(message, ex);
            }

            PreStartWebApplication();

            isStarted = true;
        }

        public static void PreStartWebApplication()
        {
            if (isStarted)
            {
                return;
            }

            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                logger.Info("Registering per web request lifetime manager module...");
                PerWebRequestLifetimeModule.DynamicModuleRegistration();
            }
            catch (Exception ex)
            {
                string message = "Failed to register per web request lifetime manager module.";
                logger.Fatal(message, ex);

                throw new PlatformException(message, ex);
            }

            try
            {
                logger.Info("Load assemblies...");
                WebApplicationContext.LoadAssemblies();
            }
            catch (Exception ex)
            {
                string message = "Failed to load assemblies.";
                logger.Fatal(message, ex);

                throw new PlatformException(message, ex);
            }

            isStarted = true;
        }
    }
}
