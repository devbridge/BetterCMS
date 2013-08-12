using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace BetterCms.Configuration
{
    public class CmsConfigurationSection : ConfigurationSection, ICmsConfiguration
    {
        private const string VersionAttribute = "version";
        private const string UseMinifiedResourcesAttribute = "useMinifiedResources";
        private const string ResourcesBasePathAttribute = "resourcesBasePath";
        private const string LoginUrlAttribute = "loginUrl";
        private const string PageNotFoundUrlAttribute = "pageNotFoundUrl";
        private const string UrlModeAttribute = "urlMode";
        private const string DatabaseNode = "database";
        private const string StorageNode = "storage";
        private const string CacheNode = "cache";
        private const string SecurityNode = "security";
        private const string ApiNode = "api";
        private const string ModuleGalleryNode = "moduleGallery";
        private const string WorkingDirectoryRootPathAttribute = "workingDirectoryRootPath";
        private const string ArticleUrlPatternAttribute = "articleUrlPattern";
        private const string UrlPatternsNode = "urlPatterns";
        private const string InstallationNode = "installation";
        private const string AccessControlEnabledAttribute = "accessControlEnabled";
        private const string DefaultAccessControlListNode = "accessControlList";

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
        /// Gets or sets a value indicating whether [access control enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [access control enabled]; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(AccessControlEnabledAttribute, IsRequired = false, DefaultValue = false)]
        public bool AccessControlEnabled
        {
            get { return (bool)this[AccessControlEnabledAttribute]; }
            set { this[AccessControlEnabledAttribute] = value; }
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

        [ConfigurationProperty(DefaultAccessControlListNode, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(AccessControlCollection))]
        public AccessControlCollection DefaultAccessControlList
        {
            get
            {
                return this[DefaultAccessControlListNode] as AccessControlCollection;
            }
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

        ICmsStorageConfiguration ICmsConfiguration.Storage
        {
            get { return Storage; }
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
