using System;
using System.Collections.Generic;
using System.Reflection;
using BetterModules.Core.Web.Configuration;

namespace BetterCms.Configuration
{
    public class CmsConfigurationSection : DefaultWebConfigurationSection//, ICmsConfiguration
    {
        public CmsConfigurationSection()
        {
            UrlPatterns = new List<PatternElement>();
            Modules = new List<ModuleElement>();
        }

        /// <summary>
        /// The version backing field.
        /// </summary>
        private string version;

        #region Attributes

        /// <summary>
        /// Gets the Better CMS version.
        /// </summary>
        /// <value>
        /// The Better CMS version.
        /// </value>
        public string Version
        {
            get
            {
                //if (string.IsNullOrEmpty(version))
                //{
                //    if (this[VersionAttribute] != null)
                //    {
                //        version = this[VersionAttribute].ToString();
                //    }
                //}

                if (string.IsNullOrEmpty(version))
                {
                    var assemblyInformationVersion = Attribute.GetCustomAttributes(GetType().Assembly, typeof(AssemblyInformationalVersionAttribute)); 
                    if (assemblyInformationVersion.Length > 0)
                    {
                        version = ((AssemblyInformationalVersionAttribute)assemblyInformationVersion[0]).InformationalVersion;
                    }
                    else
                    {
                        version = GetType().Assembly.GetName().Version.ToString(4);
                    }
                }

                return version;
            }
            set { version = value; }
        }

        /// <summary>
        /// Gets or sets the login URL.
        /// </summary>
        /// <value>
        /// The login URL.
        /// </value>
        public string LoginUrl { get; set; } = "";

        /// <summary>
        /// Gets the virtual root path (like "~/App_Data") of BetterCMS working directory. 
        /// </summary>
        /// <value>
        /// The virtual root path of BetterCMS working directory.
        /// </value>
        public string WorkingDirectoryRootPath { get; set; }

        /// <summary>
        /// Gets a value indicating whether CMS should use minified resources (*.min.js and *.min.css).
        /// </summary>
        /// <value>
        /// <c>true</c> if CMS should use minified resources; otherwise, <c>false</c>.
        /// </value>
        public bool UseMinifiedResources { get; set; } = false;

        /// <summary>
        /// Gets the CMS resources (*.js and *.css) base path.
        /// </summary>
        /// <value>
        /// The CMS content base path.
        /// </value>
        public string ResourcesBasePath { get; set; }

        /// <summary>
        /// Gets or sets the page not found layout.
        /// </summary>
        /// <value>
        /// The page not found layout.
        /// </value>
        public string PageNotFoundUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL mode.
        /// </summary>
        /// <value>
        /// The URL mode.
        /// </value>
        public TrailingSlashBehaviorType UrlMode { get; set; } = TrailingSlashBehaviorType.TrailingSlash;

        /// <summary>
        /// Gets or sets the article url prefix.
        /// </summary>
        /// <value>
        /// The article url prefix.
        /// </value>
        public string ArticleUrlPattern { get; set; }

        /// <summary>
        /// Gets a value indicating whether to render content ending div.
        /// </summary>
        /// <value>
        /// <c>true</c> if to render content ending div; otherwise, <c>false</c>.
        /// </value>
        public bool RenderContentEndingDiv { get; set; } = true;

        /// <summary>
        /// Gets the name of the content ending div CSS class.
        /// </summary>
        /// <value>
        /// The name of the content ending div CSS class.
        /// </value>
        public string ContentEndingDivCssClassName { get; set; } = "custom-clearfix";

        /// <summary>
        /// Gets a value indicating whether to enable multilanguage.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to enable multilanguage; otherwise, <c>false</c>.
        /// </value>
        public bool EnableMultilanguage { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether macros are enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if macros are enabled; otherwise, <c>false</c>.
        /// </value>
        public bool EnableMacros { get; set; } = false;

        #endregion

        #region Child Nodes

        /// <summary>
        /// Gets or sets the URL patterns.
        /// </summary>
        /// <value>
        /// The URL patterns.
        /// </value>
        public IList<PatternElement> UrlPatterns { get; set; }
        
        public IList<ModuleElement> Modules { get; set; }

        /// <summary>
        /// Gets or sets the configuration of CMS storage service.
        /// </summary>
        /// <value>
        /// The storage service configuration.
        /// </value>
        public CmsStorageConfigurationElement Storage { get; set; }
        
        public CmsSearchConfigurationElement Search { get; set; }
        
        public CmsCacheConfigurationElement Cache { get; set; }
        
        public CmsSecurityConfigurationElement Security { get; set; }        
        
        public CmsModuleGalleryConfigurationElement ModuleGallery { get; set; }
        
        public CmsInstallationConfigurationElement Installation { get; set; }

        public CmsUsersConfigurationElement Users { get; set; }

        #endregion
    }
}
