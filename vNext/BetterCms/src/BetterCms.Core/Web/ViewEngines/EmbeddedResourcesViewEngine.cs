using System.Web.Mvc;

namespace BetterCms.Core.Web.ViewEngines
{
    /// <summary>
    /// Embedded resources view engine.
    /// </summary>
    public class EmbeddedResourcesViewEngine : RazorViewEngine
    {
        ///// <summary>
        ///// Initializes a new instance of the <see cref="EmbeddedResourcesViewEngine" /> class.
        ///// </summary>
        //public EmbeddedResourcesViewEngine()
        //{
        //    AreaViewLocationFormats = new[]
        //        {
        //            "~/Areas/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{2}/Views/{1}/{0}.cshtml", 
        //            "~/Areas/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{2}/Views/{1}/{0}.vbhtml",
        //            "~/Areas/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{2}/Views/Shared/{0}.cshtml", 
        //            "~/Areas/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{2}/Views/Shared/{0}.vbhtml"
        //        };
            
        //    AreaMasterLocationFormats = new[]
        //        {
        //            "~/Areas/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{2}/Views/{1}/{0}.cshtml",
        //            "~/Areas/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{2}/Views/{1}/{0}.vbhtml", 
        //            "~/Areas/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{2}/Views/Shared/{0}.cshtml", 
        //            "~/Areas/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{2}/Views/Shared/{0}.vbhtml"
        //        };
            
        //    AreaPartialViewLocationFormats = new[]
        //        {
        //            "~/Areas/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{2}/Views/{1}/{0}.cshtml", 
        //            "~/Areas/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{2}/Views/{1}/{0}.vbhtml",
        //            "~/Areas/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{2}/Views/Shared/{0}.cshtml", 
        //            "~/Areas/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{2}/Views/Shared/{0}.vbhtml"
        //        };
            
        //    ViewLocationFormats = new[]
        //        {
        //            "~/Views/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{1}/{0}.cshtml", 
        //            "~/Views/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{1}/{0}.vbhtml", 
        //            "~/Views/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/Shared/{0}.cshtml",
        //            "~/Views/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/Shared/{0}.vbhtml"
        //        };
            
        //    MasterLocationFormats = new[]
        //        {
        //            "~/Views/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{1}/{0}.cshtml", 
        //            "~/Views/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{1}/{0}.vbhtml", 
        //            "~/Views/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/Shared/{0}.cshtml", 
        //            "~/Views/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/Shared/{0}.vbhtml"
        //        };
            
        //    PartialViewLocationFormats = new[]
        //        { 
        //            "~/Views/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{1}/{0}.cshtml", 
        //            "~/Views/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/{1}/{0}.vbhtml", 
        //            "~/Views/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/Shared/{0}.cshtml",
        //            "~/Views/" + DefaultEmbeddedResourcesProvider.EmbeddedResourcePrefix + "/Shared/{0}.vbhtml"
        //        };

        //    FileExtensions = new[] { "cshtml", "vbhtml" };
        //}

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return base.FileExists(controllerContext, virtualPath);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return base.FindPartialView(controllerContext, partialViewName, useCache);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return base.CreatePartialView(controllerContext, partialPath);
        }
    }
}
