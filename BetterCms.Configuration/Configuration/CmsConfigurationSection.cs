﻿using System;
using System.Configuration;
using System.Reflection;

namespace BetterCms.Configuration
{
    public class CmsConfigurationSection : ConfigurationSection, ICmsConfiguration
    {
        protected const string VersionAttribute = "version";
        protected const string UseMinifiedResourcesAttribute = "useMinifiedResources";
        protected const string ResourcesBasePathAttribute = "resourcesBasePath";
        protected const string LoginUrlAttribute = "loginUrl";
        protected const string WebSiteUrlAttribute = "webSiteUrl";
        protected const string PageNotFoundUrlAttribute = "pageNotFoundUrl";
        protected const string UrlModeAttribute = "urlMode";
        protected const string DatabaseNode = "database";
        protected const string StorageNode = "storage";
        protected const string SearchNode = "search";
        protected const string CacheNode = "cache";
        protected const string SecurityNode = "security";
        protected const string ModuleGalleryNode = "moduleGallery";
        protected const string WorkingDirectoryRootPathAttribute = "workingDirectoryRootPath";
        protected const string ArticleUrlPatternAttribute = "articleUrlPattern";
        protected const string UrlPatternsNode = "urlPatterns";
        protected const string InstallationNode = "installation";
        protected const string UsersNode = "users";
        protected const string RenderContentEndingDivAttribute = "renderContentEndingDiv";
        protected const string ContentEndingDivCssClassNameAttribute = "contentEndingDivCssClassName";
        protected const string EnableMultilanguageAttribute = "enableMultilanguage";
        protected const string EnableMacrosAttribute = "enableMacros";

        /// <summary>
        /// The version backing field.
        /// </summary>
        protected string version;

        #region Attributes

        /// <summary>
        /// Gets the Better CMS version.
        /// </summary>
        /// <value>
        /// The Better CMS version.
        /// </value>
        [ConfigurationProperty(VersionAttribute, DefaultValue = null, IsRequired = false)]
        public string Version
        {
            get
            {
                if (string.IsNullOrEmpty(version))
                {
                    if (this[VersionAttribute] != null)
                    {
                        version = this[VersionAttribute].ToString();
                    }
                }

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
            set { this[VersionAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the login URL.
        /// </summary>
        /// <value>
        /// The login URL.
        /// </value>
        [ConfigurationProperty(LoginUrlAttribute, DefaultValue = "", IsRequired = false)]
        public string LoginUrl
        {
            get { return Convert.ToString(this[LoginUrlAttribute]); }
            set { this[LoginUrlAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the web site URL.
        /// </summary>
        /// <value>
        /// The web site URL.
        /// </value>
        [ConfigurationProperty(WebSiteUrlAttribute, DefaultValue = "Auto", IsRequired = false)]
        public string WebSiteUrl
        {
            get { return Convert.ToString(this[WebSiteUrlAttribute]); }
            set { this[WebSiteUrlAttribute] = value; }
        }

        /// <summary>
        /// Gets the virtual root path (like "~/App_Data") of BetterCMS working directory. 
        /// </summary>
        /// <value>
        /// The virtual root path of BetterCMS working directory.
        /// </value>        [ConfigurationProperty(WorkingDirectoryRootPathAttribute, DefaultValue = "", IsRequired = false)]
        [ConfigurationProperty(WorkingDirectoryRootPathAttribute, IsRequired = true)]
        public string WorkingDirectoryRootPath
        {
            get { return Convert.ToString(this[WorkingDirectoryRootPathAttribute]); }
            set { this[WorkingDirectoryRootPathAttribute] = value; }
        }

        /// <summary>
        /// Gets a value indicating whether CMS should use minified resources (*.min.js and *.min.css).
        /// </summary>
        /// <value>
        /// <c>true</c> if CMS should use minified resources; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(UseMinifiedResourcesAttribute, IsRequired = false, DefaultValue = false)]
        public bool UseMinifiedResources
        {
            get { return Convert.ToBoolean(this[UseMinifiedResourcesAttribute]); }
            set { this[UseMinifiedResourcesAttribute] = value; }
        }

        /// <summary>
        /// Gets the CMS resources (*.js and *.css) base path.
        /// </summary>
        /// <value>
        /// The CMS content base path.
        /// </value>
        [ConfigurationProperty(ResourcesBasePathAttribute, IsRequired = false, DefaultValue = null)]
        public string ResourcesBasePath
        {
            get { return Convert.ToString(this[ResourcesBasePathAttribute]); }
            set { this[ResourcesBasePathAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the page not found layout.
        /// </summary>
        /// <value>
        /// The page not found layout.
        /// </value>
        [ConfigurationProperty(PageNotFoundUrlAttribute, IsRequired = false)]
        public string PageNotFoundUrl
        {
            get { return Convert.ToString(this[PageNotFoundUrlAttribute]); }
            set { this[PageNotFoundUrlAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the URL mode.
        /// </summary>
        /// <value>
        /// The URL mode.
        /// </value>
        [ConfigurationProperty(UrlModeAttribute, IsRequired = false, DefaultValue = TrailingSlashBehaviorType.TrailingSlash)]
        public TrailingSlashBehaviorType UrlMode
        {
            get { return (TrailingSlashBehaviorType)this[UrlModeAttribute]; }
            set { this[UrlModeAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the article url prefix.
        /// </summary>
        /// <value>
        /// The article url prefix.
        /// </value>
        [ConfigurationProperty(ArticleUrlPatternAttribute, IsRequired = false)]
        public string ArticleUrlPattern {
            get {return Convert.ToString(this[ArticleUrlPatternAttribute]); }
            set { this[ArticleUrlPatternAttribute] = value; }
        }

        /// <summary>
        /// Gets a value indicating whether to render content ending div.
        /// </summary>
        /// <value>
        /// <c>true</c> if to render content ending div; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(RenderContentEndingDivAttribute, IsRequired = false, DefaultValue = true)]
        public bool RenderContentEndingDiv
        {
            get { return (bool)this[RenderContentEndingDivAttribute]; }
            set { this[RenderContentEndingDivAttribute] = value; }
        }

        /// <summary>
        /// Gets the name of the content ending div CSS class.
        /// </summary>
        /// <value>
        /// The name of the content ending div CSS class.
        /// </value>
        [ConfigurationProperty(ContentEndingDivCssClassNameAttribute, IsRequired = false, DefaultValue = "custom-clearfix")]
        public string ContentEndingDivCssClassName
        {
            get { return Convert.ToString(this[ContentEndingDivCssClassNameAttribute]); }
            set { this[ContentEndingDivCssClassNameAttribute] = value; }
        }

        /// <summary>
        /// Gets a value indicating whether to enable multilanguage.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to enable multilanguage; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(EnableMultilanguageAttribute, IsRequired = false, DefaultValue = true)]
        public bool EnableMultilanguage
        {
            get { return (bool)this[EnableMultilanguageAttribute]; }
            set { this[EnableMultilanguageAttribute] = value; }
        }

        /// <summary>
        /// Gets a value indicating whether macros are enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if macros are enabled; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(EnableMacrosAttribute, IsRequired = false, DefaultValue = false)]
        public bool EnableMacros
        {
            get { return (bool)this[EnableMacrosAttribute]; }
            set { this[EnableMacrosAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the URL patterns.
        /// </summary>
        /// <value>
        /// The URL patterns.
        /// </value>
        [ConfigurationProperty(UrlPatternsNode, IsRequired = false)]
        public UrlPatternsCollection UrlPatterns
        {
            get { return (UrlPatternsCollection)this[UrlPatternsNode]; }
            set { this[UrlPatternsNode] = value; }
        }

        #endregion

        #region Child Nodes
        
        /// <summary>
        /// Gets or sets the configuration of CMS storage service.
        /// </summary>
        /// <value>
        /// The storage service configuration.
        /// </value>
        [ConfigurationProperty(StorageNode, IsRequired = true)]
        public CmsStorageConfigurationElement Storage
        {
            get { return (CmsStorageConfigurationElement)this[StorageNode]; }
            set { this[StorageNode] = value; }
        }

        [ConfigurationProperty(SearchNode, IsRequired = false)]
        public CmsSearchConfigurationElement Search
        {
            get { return (CmsSearchConfigurationElement)this[SearchNode]; }
            set { this[SearchNode] = value; }
        }

        [ConfigurationProperty(CacheNode, IsRequired = true)]
        public CmsCacheConfigurationElement Cache
        {
            get { return (CmsCacheConfigurationElement)this[CacheNode]; }
            set { this[CacheNode] = value; }
        }

        [ConfigurationProperty(DatabaseNode, IsRequired = true)]
        public CmsDatabaseConfigurationElement Database
        {
            get { return (CmsDatabaseConfigurationElement)this[DatabaseNode]; }
            set { this[DatabaseNode] = value; }
        }

        [ConfigurationProperty(SecurityNode, IsRequired = true)]
        public CmsSecurityConfigurationElement Security
        {
            get { return (CmsSecurityConfigurationElement)this[SecurityNode]; }
            set { this[SecurityNode] = value; }
        }        

        [ConfigurationProperty(ModuleGalleryNode, IsRequired = true)]
        public CmsModuleGalleryConfigurationElement ModuleGallery
        {
            get { return (CmsModuleGalleryConfigurationElement)this[ModuleGalleryNode]; }
            set { this[ModuleGalleryNode] = value; }
        }

        [ConfigurationProperty(InstallationNode, IsRequired = false)]
        public CmsInstallationConfigurationElement Installation
        {
            get { return (CmsInstallationConfigurationElement)this[InstallationNode]; }
            set { this[InstallationNode] = value; }
        }

        [ConfigurationProperty(UsersNode, IsRequired = false)]
        public CmsUsersConfigurationElement Users
        {
            get { return this[UsersNode] as CmsUsersConfigurationElement; }
            set { this[UsersNode] = value; }
        }

        ICmsUsersConfiguration ICmsConfiguration.Users
        {
            get { return Users; }
        }

        ICmsStorageConfiguration ICmsConfiguration.Storage
        {
            get { return Storage; }
        }

        ICmsSearchConfiguration ICmsConfiguration.Search
        {
            get { return Search; }
        }


        ICmsCacheConfiguration ICmsConfiguration.Cache
        {
            get { return Cache; }
        }

        ICmsDatabaseConfiguration ICmsConfiguration.Database
        {
            get { return Database; }
        }

        ICmsSecurityConfiguration ICmsConfiguration.Security
        {
            get { return Security; }
        }

        ICmsModuleGalleryConfiguration ICmsConfiguration.ModuleGallery
        {
            get { return ModuleGallery; }
        }

        ICmsInstallationConfiguration ICmsConfiguration.Installation
        {
            get { return Installation; }
        }

        #endregion
    }
}
