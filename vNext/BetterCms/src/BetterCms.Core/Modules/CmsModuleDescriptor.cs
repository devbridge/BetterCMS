using System;
using System.Collections.Generic;

using Autofac;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Projections;

using BetterModules.Core.Web.Modules;
using BetterModules.Core.Web.Mvc.Extensions;

namespace BetterCms.Core.Modules
{
    /// <summary>
    /// Abstract module descriptor. 
    /// </summary>
    public abstract class CmsModuleDescriptor : WebModuleDescriptor
    {
        private string areaName;

        private string baseModulePath;

        private string minJsPath;

        private string minCssPath;

        private string baseJsPath;

        private string baseCssPath;

        private const string cmsSchemaBase = "bcms_{0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsModuleDescriptor" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected CmsModuleDescriptor(ICmsConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public virtual ICmsConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets the name of the module area.
        /// </summary>
        /// <value>
        /// The name of the module area.
        /// </value>
        public override string AreaName
        {
            get
            {
                if (areaName == null)
                {
                    areaName = ("bcms-" + Name).ToLowerInvariant();
                }

                return areaName;
            }
        }

        /// <summary>
        /// Gets the base module path. Default value is /file/module-name/.
        /// </summary>
        /// <value>
        /// The base module path.
        /// </value>
        public virtual string BaseModulePath
        {
            get
            {
                if (baseModulePath == null)
                {                    
                    if (!string.IsNullOrWhiteSpace(Configuration.ResourcesBasePath) && !VirtualPath.IsLocalPath(Configuration.ResourcesBasePath))
                    {                       
                        baseModulePath = VirtualPath.Combine(Configuration.ResourcesBasePath, AreaName);

                        const string versionTag = "{bcms.version}";
                        if (baseModulePath.Contains(versionTag))
                        {
                            baseModulePath = baseModulePath.Replace(versionTag, Configuration.Version);
                        }
                    }
                    else
                    {
                        if (Configuration.UseMinifiedResources)
                        {
                            throw new CmsException(
                                "The switch useMinifiedResources=\"true\" in the cms.config can't be used with local CMS resources (resourcesBasePath=\"(local)\").");
                        }
                        baseModulePath = VirtualPath.Combine("/", "file", AreaName);
                    }
                }

                return baseModulePath;
            }
        }

        public override string SchemaName
        {
            get
            {
                return string.Format(cmsSchemaBase, Name);
            }
        }

        /// <summary>
        /// Gets the path of the module packed and minified JS file.
        /// </summary>
        /// <value>
        /// The path of the module packed and minified JS file.
        /// </value>
        public virtual string MinifiedJsPath
        {
            get
            {
                if (minJsPath == null)
                {
                    minJsPath = VirtualPath.Combine(JsBasePath, string.Format("bcms.{0}.min.js", Name.ToLowerInvariant()));
                }

                return minJsPath;
            }
        }

        /// <summary>
        /// Gets the path of the module packed and minified CSS file.
        /// </summary>
        /// <value>
        /// The path of the module packed and minified CSS file.
        /// </value>
        public virtual string MinifiedCssPath
        {
            get
            {
                if (minCssPath == null)
                {
                    minCssPath = VirtualPath.Combine(CssBasePath, string.Format("bcms.{0}.min.css", Name.ToLowerInvariant()));
                }

                return minCssPath;
            }
        }

        /// <summary>
        /// Gets the JavaScript base path.
        /// </summary>
        /// <value>
        /// The JavaScript base path.
        /// </value>
        public virtual string JsBasePath
        {
            get
            {
                if (baseJsPath == null)
                {
                    baseJsPath = VirtualPath.Combine(BaseModulePath, "scripts");
                }

                return baseJsPath;
            }
        }

        /// <summary>
        /// Gets the CSS base path.
        /// </summary>
        /// <value>
        /// The CSS base path.
        /// </value>
        public virtual string CssBasePath
        {
            get
            {
                if (baseCssPath == null)
                {
                    baseCssPath = VirtualPath.Combine(BaseModulePath, "content", "styles");
                }

                return baseCssPath;
            }
        }

        /// <summary>
        /// Registers java script modules.
        /// </summary>        
        /// <returns>Enumerator of known JS modules list.</returns>
        public virtual IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return null;
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>                
        /// <returns>Enumerator of known module style sheet files.</returns>
        public virtual IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return null;
        }

        public virtual IEnumerable<IPageActionProjection> RegisterSidebarHeaderProjections(ContainerBuilder containerBuilder)
        {
            return null;
        }

        public virtual IEnumerable<IPageActionProjection> RegisterSidebarSideProjections(ContainerBuilder containerBuilde)
        {
            return null;
        }

        public virtual IEnumerable<IPageActionProjection> RegisterSidebarMainProjections(ContainerBuilder containerBuilder)
        {
            return null;
        }

        public virtual IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder)
        {
            return null;
        }

        protected void RegisterContentRendererType<TContentRenderer, TContent>(ContainerBuilder containerBuilder) 
            where TContentRenderer : ContentAccessor<TContent>
            where TContent : class, IContent
        {
            Type contentRendererType = typeof(TContentRenderer);
            Type contentType = typeof(TContent);

            string key = ("ContentRenderer-" + contentType.Name).ToUpperInvariant();
            containerBuilder
                .RegisterType(contentRendererType)
                .AsSelf()
                .Keyed<IContentAccessor>(key)
                .WithMetadata("ContentRendererType", contentRendererType)               
                .InstancePerDependency();
        }

        protected void RegisterJavaScriptRendererType<TJavaScriptRenderer, TContent>(ContainerBuilder containerBuilder)
            where TJavaScriptRenderer : IJavaScriptAccessor
            where TContent : class
        {
            Type jsRendererType = typeof(TJavaScriptRenderer);
            Type contentType = typeof(TContent);

            string key = ("JavaScriptRenderer-" + contentType.Name).ToUpperInvariant();
            containerBuilder
                .RegisterType(jsRendererType)
                .AsSelf()
                .Keyed<IJavaScriptAccessor>(key)
                .WithMetadata("JavaScriptRendererType", jsRendererType)
                .InstancePerDependency();
        }
        
        protected void RegisterStylesheetRendererType<TStyleSheetRenderer, TContent>(ContainerBuilder containerBuilder)
            where TStyleSheetRenderer : IStylesheetAccessor
            where TContent : class
        {
            Type styleRendererType = typeof(TStyleSheetRenderer);
            Type contentType = typeof(TContent);

            string key = ("StyleSheetRenderer-" + contentType.Name).ToUpperInvariant();
            containerBuilder
                .RegisterType(styleRendererType)
                .AsSelf()
                .Keyed<IStylesheetAccessor>(key)
                .WithMetadata("StylesheetRendererType", styleRendererType)
                .InstancePerDependency();
        }
    }
}
