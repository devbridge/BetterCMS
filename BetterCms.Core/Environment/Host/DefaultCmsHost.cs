using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Routing;
using System.Web.Mvc;

using BetterCms.Api;
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Environment.Assemblies;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Host;
using BetterCms.Core.Modules.Registration;

using Common.Logging;

namespace BetterCms.Core.Environment.Host
{
    /// <summary>
    /// Default BetterCMS host implementation.
    /// </summary>
    public class DefaultCmsHost : ICmsHost
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// A field to store current CMS version.
        /// </summary>
        private volatile Version currentVersion = null;

        /// <summary>
        ///
        /// </summary>
        private readonly IModulesRegistration modulesRegistration;

        private readonly IMigrationRunner migrationRunner;

        public DefaultCmsHost(IModulesRegistration modulesRegistration, IMigrationRunner migrationRunner)
        {
            this.modulesRegistration = modulesRegistration;
            this.migrationRunner = migrationRunner;
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public Version Version
        {
            get
            {
                if (currentVersion == null)
                {
                    lock (this)
                    {
                        if (currentVersion == null)
                        {
                            currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                        }
                    }
                }

                return currentVersion;
            }
        }

        /// <summary>
        /// Called on host application start.
        /// </summary>
        /// <param name="application">The host application.</param>
        public void OnApplicationStart(HttpApplication application)
        {
            try
            {
                Logger.Info("BetterCMS host application starting...");

                modulesRegistration.RegisterKnownModuleRoutes(RouteTable.Routes);
                MigrateDatabase();
                
                // Notify.
                ApiContext.Events.OnHostStart(this);

                Logger.Info("BetterCMS host application started.");
            }
            catch (Exception ex)
            {
                Logger.Fatal("Failed to start BetterCMS host application.", ex);
            }
        }

        /// <summary>
        /// Called on host application end.
        /// </summary>
        /// <param name="application">The host application.</param>
        public void OnApplicationEnd(HttpApplication application)
        {
            Logger.Info("BetterCMS host application stopped.");
        }

        /// <summary>
        /// Called on host application error.
        /// </summary>
        /// <param name="application">The host application.</param>
        public void OnApplicationError(HttpApplication application)
        {
            var error = application.Server.GetLastError();
            Logger.Fatal("Unhandled exception occurred in BetterCMS host application.", error);
        }

        /// <summary>
        /// Called when host ends web request.
        /// </summary>
        /// <param name="application">The host application.</param>
        public void OnEndRequest(HttpApplication application)
        {
        }

        /// <summary>
        /// Called when host begins web request.
        /// </summary>
        /// <param name="application">The host application.</param>
        public void OnBeginRequest(HttpApplication application)
        {
#if DEBUG
            if (application.Request["restart"] == "1")
            {
                RestartAndReloadHost(application);
            }
#endif
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
                Logger.Warn("Failed to touch BetterCMS host application web.config file.", ex);
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
                Logger.Warn("Failed to touch BetterCMS host application \bin folder.", ex);
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
                migrationRunner.Migrate(descriptors, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}