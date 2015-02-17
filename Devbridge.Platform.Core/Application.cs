using System;

using Common.Logging;

using Devbridge.Platform.Core.Dependencies;
using Devbridge.Platform.Core.Exceptions;

namespace Devbridge.Platform.Core
{
    /// <summary>
    /// Application entry point to run application preload  logic. 
    /// </summary>
    public static class Application
    {
        /// <summary>
        /// Method to run logic when an application starts.
        /// </summary>    
        public static void Initialize()
        {
            ILog logger;

            try
            {
                logger = LogManager.GetCurrentClassLogger();
                logger.Info("Starting Application...");
            }
            catch (Exception ex)
            {
                throw new PlatformException("Logging is not working. A reason may be that Common.Logging section is not configured in web.config.", ex);
            } 

            try
            {
                logger.Info("Creating Application context dependencies container...");
                ContextScopeProvider.RegisterTypes(ApplicationContext.InitializeContainer());
            }
            catch (Exception ex)
            {
                string message = "Failed to create Web Application context dependencies container.";
                logger.Fatal(message, ex);

                throw new PlatformException(message, ex);
            }

            try
            {
                logger.Info("Load assemblies...");
                ApplicationContext.LoadAssemblies();
            }
            catch (Exception ex)
            {
                string message = "Failed to load assemblies.";
                logger.Fatal(message, ex);

                throw new PlatformException(message, ex);
            }
        }

        /// <summary>
        /// Initializes the logger.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformException">Logging is not working. A reason may be that Common.Logging section is not configured in web.config.</exception>
        public static ILog InitializeLogger()
        {
            try
            {
                return LogManager.GetCurrentClassLogger();
            }
            catch (Exception ex)
            {
                throw new PlatformException("Logging is not working. A reason may be that Common.Logging section is not configured in web.config.", ex);
            }            
        }
    }
}
