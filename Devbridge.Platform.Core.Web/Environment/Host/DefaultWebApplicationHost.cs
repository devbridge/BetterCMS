using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

using Common.Logging;

using Devbridge.Platform.Core.DataAccess.DataContext.Migrations;
using Devbridge.Platform.Core.Exceptions;
using Devbridge.Platform.Core.Web.Exceptions.Host;
using Devbridge.Platform.Core.Web.Modules.Registration;

using RazorGenerator.Mvc;

namespace Devbridge.Platform.Core.Web.Environment.Host
{
    /// <summary>
    /// Default web host implementation.
    /// </summary>
    public class DefaultWebApplicationHost : IWebApplicationHost
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private readonly IWebModulesRegistration modulesRegistration;

        private readonly IMigrationRunner migrationRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWebApplicationHost" /> class.
        /// </summary>
        /// <param name="modulesRegistration">The modules registration.</param>
        /// <param name="migrationRunner">The migration runner.</param>
        public DefaultWebApplicationHost(IWebModulesRegistration modulesRegistration, IMigrationRunner migrationRunner)
        {
            this.modulesRegistration = modulesRegistration;
            this.migrationRunner = migrationRunner;
        }

        /// <summary>
        /// Called when the host application starts.
        /// </summary>
        /// <param name="application">The host application.</param>
        /// <param name="validateViewEngines">if set to <c>true</c> valdiate view engines.</param>
        /// <exception cref="PlatformException">ViewEngines.Engines collection doesn't contain any precompiled MVC view engines. Each module uses precompiled MVC engines for rendering views. Please check if Engines list is not cleared manualy in global.asax.cx</exception>
        public virtual void OnApplicationStart(HttpApplication application, bool validateViewEngines = true)
        {
            try
            {
                Logger.Info("Web application host starting...");

                if (validateViewEngines && !ViewEngines.Engines.Any(engine => engine is CompositePrecompiledMvcEngine))
                {
                    throw new PlatformException("ViewEngines.Engines collection doesn't contain precompiled composite MVC view engine. Application modules use precompiled MVC views for rendering. Please check if Engines list is not cleared manualy in global.asax.cx");
                }

                modulesRegistration.RegisterKnownModuleRoutes(RouteTable.Routes);
                MigrateDatabase();

                // Notify.                                
                Events.WebCoreEvents.Instance.OnHostStart(application);

                Logger.Info("Web application host started.");
            }
            catch (Exception ex)
            {
                Logger.Fatal("Failed to start host application.", ex);
            }
        }

        /// <summary>
        /// Called when the host application stops.
        /// </summary>
        /// <param name="application">The host application.</param>
        public virtual void OnApplicationEnd(HttpApplication application)
        {
            Logger.Info("Web host application stopped.");
            
            // Notify.
            Events.WebCoreEvents.Instance.OnHostStop(application);
        }

        /// <summary>
        /// Called when the host application throws unhandled error.
        /// </summary>
        /// <param name="application">The host application.</param>
        public virtual void OnApplicationError(HttpApplication application)
        {
            var error = application.Server.GetLastError();
            Logger.Fatal("Unhandled exception occurred in web host application.", error);

            // Notify.
            Events.WebCoreEvents.Instance.OnHostError(application);
        }
        
        /// <summary>
        /// Called when the host application ends a web request.
        /// </summary>
        /// <param name="application">The host application.</param>
        public virtual void OnEndRequest(HttpApplication application)
        {
        }
        
        /// <summary>
        /// Called when the host application begins a web request.
        /// </summary>
        /// <param name="application">The host application.</param>
        public virtual void OnBeginRequest(HttpApplication application)
        {
#if DEBUG
            // A quick way to restart an application host.
            // This is not going to affect production as it is compiled only in the debug mode.
            if (application.Request["restart"] == "1")
            {
                RestartAndReloadHost(application);
            }
#endif
        }

        /// <summary>
        /// Called when the host application authenticates a web request.
        /// </summary>
        /// <param name="application"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void OnAuthenticateRequest(HttpApplication application)
        {
            // Notify.
            Events.WebCoreEvents.Instance.OnHostAuthenticateRequest(application);
        }

        /// <summary>
        /// Restarts and reloads application.
        /// </summary>
        /// <param name="application">The application.</param>
        public virtual void RestartAndReloadHost(HttpApplication application)
        {
            RestartApplicationHost();

            Thread.Sleep(500);

            UriBuilder uri = new UriBuilder(application.Request.Url);
            uri.Query = string.Empty;

            application.Response.ClearContent();
            application.Response.Write(string.Format("<script type=\"text/javascript\">window.location = '{0}';</script>", uri));
            application.Response.End();
        }

        /// <summary>
        /// Terminates current application. The application restarts on the next time a request is received for it.
        /// </summary>
        public virtual void RestartApplicationHost()
        {
            try
            {
                HttpRuntime.UnloadAppDomain();
            }
            catch
            {
                try
                {
                    bool success = TryTouchBinRestartMarker() || TryTouchWebConfig();

                    if (!success)
                    {
                        throw new RestartApplicationException("Failed to terminate host application.");
                    }
                }
                catch (Exception ex)
                {
                    throw new RestartApplicationException("Failed to terminate host application.", ex);
                }
            }
        }

        /// <summary>
        /// Tries to update last write time for web configuration file to restart application.
        /// </summary>
        /// <returns><c>true</c> if web.config file updated successfully; otherwise, <c>false</c>. </returns>
        private bool TryTouchWebConfig()
        {
            try
            {
                File.SetLastWriteTimeUtc(HostingEnvironment.MapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Warn("Failed to touch web host application web.config file.", ex);
                return false;
            }
        }

        /// <summary>
        /// Tries the touch restart marker in ~/bin/restart folder.
        /// </summary>
        /// <returns><c>true</c> if file updated successfully; otherwise, <c>false</c>. </returns>
        private bool TryTouchBinRestartMarker()
        {
            try
            {
                var binMarker = HostingEnvironment.MapPath("~/bin/restart");
                Directory.CreateDirectory(binMarker);

                using (var stream = File.CreateText(Path.Combine(binMarker, "marker.txt")))
                {
                    stream.WriteLine("Restarted on '{0}'", DateTime.UtcNow);
                    stream.Flush();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Warn("Failed to touch web host application \bin folder.", ex);
                return false;
            }
        }

        /// <summary>
        /// Updates database.
        /// </summary>        
        private void MigrateDatabase()
        {
            try
            {
                var descriptors = modulesRegistration.GetModules().Select(m => m.ModuleDescriptor).ToList();                
                migrationRunner.MigrateStructure(descriptors);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}