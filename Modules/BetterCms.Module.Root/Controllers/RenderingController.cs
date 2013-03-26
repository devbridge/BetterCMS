using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Registration;
using BetterCms.Core.Mvc.Attributes;
using BetterCms.Core.Services;

using BetterCms.Module.Root.Models.Rendering;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels;
using BetterCms.Module.Root.ViewModels.Rendering;

using Common.Logging;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Script handling controller.
    /// </summary>
    public class RenderingController : CmsControllerBase
    {        
        /// <summary>
        /// Current class logger.
        /// </summary>       
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// A contract to manage modules registry.
        /// </summary>
        private readonly IModulesRegistration modulesRegistration;

        /// <summary>
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingController" /> class.
        /// </summary>
        /// <param name="modulesRegistration">A contract to manage modules registry.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public RenderingController(IModulesRegistration modulesRegistration, ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
            this.modulesRegistration = modulesRegistration;
        }

        /// <summary>
        /// Renders bcms.main.js or bcms.main.min.js (entry point of the BetterCMS client side).
        /// </summary>
        /// <returns>main.js or main.min.js file with client side entry point.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        //[OutputCache(Duration = 120, Location = OutputCacheLocation.ServerAndClient)]
        [IgnoreAutoRoute]
        public ActionResult RenderMainJsFile()
        {
            SetNoCache();

            RenderMainJsViewModel model = new RenderMainJsViewModel();
            model.JavaScriptModules = RetrieveJsModulesModel();
            model.Version = cmsConfiguration.Version;
            model.UseMinReferences = cmsConfiguration.UseMinifiedResources;
            
#if (DEBUG)
            model.IsDebugMode = true;
#endif
            return View(model, "text/javascript");
        }

        /// <summary>
        /// Renders bcms.processor.js file.
        /// </summary>
        /// <returns>bcms.processor.js file with logic to initialize and manage JS modules.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1616:ElementReturnValueDocumentationMustHaveText", Justification = "Reviewed. Suppression is OK here.")]
        [IgnoreAutoRoute]
        public ActionResult RenderProcessorJsFile()
        {
            SetNoCache();

            IEnumerable<JavaScriptModuleViewModel> model = RetrieveJsModulesModel();
          
            return View(model, "text/javascript");
        }

        /// <summary>
        /// Renders style sheet includes of registered modules.
        /// </summary>
        /// <returns>List of style sheet includes.</returns>
        public ActionResult RenderStyleSheetIncludes()
        {
            SetNoCache();

            RenderStyleSheetIncludesViewModel model;

            try
            {
                var styleSheetFiles = modulesRegistration.GetStyleSheetFiles();
                model = new RenderStyleSheetIncludesViewModel(styleSheetFiles);
            }
            catch (CmsException ex)
            {
                Log.Error("Failed to read style sheet files collection.", ex);
                model = new RenderStyleSheetIncludesViewModel();
            }

            return PartialView(model);
        }

        /// <summary>
        /// Sets current HttpContext to NoCache state.
        /// </summary>
        private void SetNoCache()
        {
            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoStore();
        }
        /// <summary>
        /// Retrieves registered java script modules model.
        /// </summary>
        /// <returns>Enumerator of JavaScriptModuleViewModel objects.</returns>
        private IEnumerable<JavaScriptModuleViewModel> RetrieveJsModulesModel()
        {
            IEnumerable<JavaScriptModuleViewModel> model = Enumerable.Empty<JavaScriptModuleViewModel>();
            try
            {
                var javaScriptModules = modulesRegistration.GetJavaScriptModules();                

                if (javaScriptModules != null)
                {
                    model = javaScriptModules
                        .Select(
                            f => new JavaScriptModuleViewModel
                                {
                                    Name = f.Name,
                                    IsAutoGenerated = f.IsAutoGenerated,
                                    Path = f.IsAutoGenerated ? string.Format(RootModuleConstants.AutoGeneratedJsFilePathPattern, f.Name) : f.Path,
                                    MinifiedPath = f.MinPath ?? f.Module.MinifiedJsPath,
                                    FriendlyName = f.FriendlyName,
                                    Links = new ProjectionsViewModel
                                        {                                            
                                            Projections = f.Links.OrderBy(x => x.Order)
                                        },
                                    Globalization = new ProjectionsViewModel
                                        {                                            
                                            Projections = f.Globalization.OrderBy(x => x.Order)
                                        }
                                });
                }
            }
            catch (CmsException ex)
            {
                Log.Error("Failed to retrieve java script modules.", ex);
            }

            return model;
        }
    }
}
