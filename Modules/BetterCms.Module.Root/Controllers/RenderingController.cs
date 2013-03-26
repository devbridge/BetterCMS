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
using BetterCms.Module.Root.Commands.GetMainJsData;
using BetterCms.Module.Root.Commands.GetProcessorJsData;
using BetterCms.Module.Root.Commands.GetStyleSheetsToRender;
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
        [IgnoreAutoRoute, NoCache]        
        public ActionResult RenderMainJsFile()
        {
            var model = GetCommand<GetMainJsDataCommand>().ExecuteCommand();
            
            return View(model, "text/javascript");
        }

        /// <summary>
        /// Renders bcms.processor.js file.
        /// </summary>
        /// <returns>bcms.processor.js file with logic to initialize and manage JS modules.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1616:ElementReturnValueDocumentationMustHaveText", Justification = "Reviewed. Suppression is OK here.")]
        [IgnoreAutoRoute, NoCache]
        public ActionResult RenderProcessorJsFile()
        {
            var model = GetCommand<GetProcessorJsDataCommand>().ExecuteCommand();
           
            return View(model, "text/javascript");
        }

        /// <summary>
        /// Renders style sheet includes of registered modules.
        /// </summary>
        /// <returns>List of style sheet includes.</returns>
        public ActionResult RenderStyleSheetIncludes()
        {
            var model = GetCommand<GetStyleSheetsToRenderCommand>().ExecuteCommand();

            return PartialView(model);
        }        
    }
}
