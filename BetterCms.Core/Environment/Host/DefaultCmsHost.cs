using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Routing;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Exceptions.Host;
using BetterCms.Core.Modules.Registration;

using Common.Logging;

namespace BetterCms.Core.Environment.Host
{
    /// <summary>
    /// Default Better CMS host implementation.
    /// </summary>
    public class DefaultCmsHost : ICmsHost
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private readonly IModulesRegistration modulesRegistration;

        private readonly IMigrationRunner migrationRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCmsHost" /> class.
        /// </summary>
        /// <param name="modulesRegistration">The modules registration.</param>
        /// <param name="migrationRunner">The migration runner.</param>
        public DefaultCmsHost(IModulesRegistration modulesRegistration, IMigrationRunner migrationRunner)
        {
            this.modulesRegistration = modulesRegistration;
            this.migrationRunner = migrationRunner;
        }

        /// <summary>
        /// Called when the host application starts.
        /// </summary>
        /// <param name="application">The host application.</param>
        public void OnApplicationStart(HttpApplication application)
        {
            try
            {
                Logger.Info("Better CMS host application starting...");

                modulesRegistration.RegisterKnownModuleRoutes(RouteTable.Routes);
                MigrateDatabase();
                
                // Notify.                                
                Events.CoreEvents.Instance.OnHostStart(application);

                Logger.Info("Better CMS host application started.");
            }
            catch (Exception ex)
            {
                Logger.Fatal("Failed to start Better CMS host application.", ex);
            }
        }

        /// <summary>
        /// Called when the host application stops.
        /// </summary>
        /// <param name="application">The host application.</param>
        public void OnApplicationEnd(HttpApplication application)
        {
            Logger.Info("Better CMS host application stopped.");
            
            // Notify.
            Events.CoreEvents.Instance.OnHostStop(application);
        }

        /// <summary>
        /// Called when the host application throws unhandled error.
        /// </summary>
        /// <param name="application">The host application.</param>
        public void OnApplicationError(HttpApplication application)
        {
            var error = application.Server.GetLastError();
            Logger.Fatal("Unhandled exception occurred in Better CMS host application.", error);

            // Notify.
            Events.CoreEvents.Instance.OnHostError(application);
        }
        
        /// <summary>
        /// Called when the host application ends a web request.
        /// </summary>
        /// <param name="application">The host application.</param>
        public void OnEndRequest(HttpApplication application)
        {
        }
        
        /// <summary>
        /// Called when the host application begins a web request.
        /// </summary>
        /// <param name="application">The host application.</param>
        public void OnBeginRequest(HttpApplication application)
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
        public void OnAuthenticateRequest(HttpApplication application)
        {
            // Notify.
            Events.CoreEvents.Instance.OnHostAuthenticateRequest(application);
        }

        /// <summary>
        /// Restarts and reloads application.
        /// </summary>
        /// <param name="application">The application.</param>
        public void RestartAndReloadHost(HttpApplication application)
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
        public void RestartApplicationHost()
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
                        throw new RestartApplicationException("Failed to terminate CMS host application.");
                    }
                }
                catch (Exception ex)
                {
                    throw new RestartApplicationException("Failed to terminate CMS host application.", ex);
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
                Logger.Warn("Failed to touch Better CMS host application web.config file.", ex);
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
                Logger.Warn("Failed to touch Better CMS host application \bin folder.", ex);
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