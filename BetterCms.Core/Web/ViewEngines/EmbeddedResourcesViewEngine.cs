// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmbeddedResourcesViewEngine.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
