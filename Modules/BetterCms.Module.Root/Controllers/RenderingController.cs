using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Registration;
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
        /// The page extensions.
        /// </summary>
        private readonly IPageAccessor pageAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingController" /> class.
        /// </summary>
        /// <param name="modulesRegistration">A contract to manage modules registry.</param>
        /// <param name="pageAccessor">The page extensions.</param>
        public RenderingController(IModulesRegistration modulesRegistration, IPageAccessor pageAccessor)
        {           
            this.modulesRegistration = modulesRegistration;
            this.pageAccessor = pageAccessor;
        }

        /// <summary>
        /// Renders main.js (entry point of requirejs) java script.
        /// </summary>
        /// <returns>main.js file with client side entry point.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        //[OutputCache(Duration = 120, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult RenderMainJsFile()
        {
            RenderMainJsViewModel model = new RenderMainJsViewModel();
            model.JavaScriptModules = RetrieveJsModulesModel();
#if (DEBUG)
            model.EnableClientSideErrorTrace = true;
#endif
            return View(model, "text/javascript");
        }

        /// <summary>
        /// Renders bcms.processor.js file.
        /// </summary>
        /// <returns>bcms.processor.js file with logic to initialize and manage JS modules.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1616:ElementReturnValueDocumentationMustHaveText", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult RenderProcessorJsFile()
        {
            IEnumerable<JavaScriptModuleViewModel> model = RetrieveJsModulesModel();
          
            return View(model, "text/javascript");
        }

        /// <summary>
        /// Renders style sheet includes of registered modules.
        /// </summary>
        /// <returns>List of style sheet includes.</returns>
        public ActionResult RenderStyleSheetIncludes()
        {
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
                                    Path = f.Path,
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
