using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

using BetterCms.Core.Mvc.Attributes;
using BetterCms.Module.Root.Commands.GetMainJsData;
using BetterCms.Module.Root.Commands.GetProcessorJsData;
using BetterCms.Module.Root.Commands.GetStyleSheetsToRender;
using BetterCms.Module.Root.Mvc;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Script handling controller.
    /// </summary>
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class RenderingController : CmsControllerBase
    {
        /// <summary>
        /// Renders bcms.main.js or bcms.main.min.js (entry point of the Better CMS client side).
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
        /// Renders all (private and public) style sheet includes of registered modules.
        /// </summary>
        /// <returns>List of style sheet includes.</returns>
        public ActionResult RenderStyleSheetIncludes()
        {
            var model = GetCommand<GetStyleSheetsToRenderCommand>().ExecuteCommand(new GetStyleSheetsToRenderRequest
                                                                                       {
                                                                                           RenderPrivateCssIncludes = true,
                                                                                           RenderPublicCssIncludes = false
                                                                                       });

            return PartialView(model);
        }

        /// <summary>
        /// Renders the module registered style sheet includes.
        /// </summary>
        /// <param name="moduleDescriptorType">Type of the module descriptor.</param>        
        public ActionResult RenderModuleStyleSheetIncludes(Type moduleDescriptorType)
        {
            var model = GetCommand<GetStyleSheetsToRenderCommand>().ExecuteCommand(new GetStyleSheetsToRenderRequest
                                                                                       {
                                                                                           RenderPrivateCssIncludes = true,
                                                                                           RenderPublicCssIncludes = true,
                                                                                           ModuleDescriptorType = moduleDescriptorType
                                                                                       });
            return PartialView("RenderStyleSheetIncludes", model);
        } 
    }
}
