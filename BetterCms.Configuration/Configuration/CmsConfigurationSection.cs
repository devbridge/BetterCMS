using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class CmsConfigurationSection : ConfigurationSection, ICmsConfiguration
    {
        private const string VersionAttribute = "version";
        private const string UseMinifiedResourcesAttribute = "useMinifiedResources";
        private const string ResourcesBasePathAttribute = "resourcesBasePath";
        private const string LoginUrlAttribute = "loginUrl";
        private const string EnforcePermissionsAttribute = "enforcePermissions";
        private const string PageNotFoundUrlAttribute = "pageNotFoundUrl";
        private const string SideMenuSectionsNode = "menuSections";
        private const string UrlPatternsNode = "urlPatterns";
        private const string DatabaseNode = "database";
        private const string StorageNode = "storage";
        private const string CacheNode = "cache";
        private const string SecurityNode = "security";
        private const string ModuleGalleryNode = "moduleGallery";
        private const string WorkingDirectoryRootPathAttribute = "workingDirectoryRootPath";
        private const string ArticleUrlPrefixAttribute = "articleUrlPrefix";        

        /// <summary>
        /// The version backing field.
        /// </summary>
        private Version version;

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
            get { return Convert.ToString(this[VersionAttribute]); }
            set { this[VersionAttribute] = value; }
        }

        Version ICmsConfiguration.Version
        {
            get
            {
                if (version == null && !string.IsNullOrEmpty(Version))
                {
                    System.Version.TryParse(Version, out version);                    
                }

                if (version == null)
                {
                    version = GetType().Assembly.GetName().Version;
                }

                return version;
            }
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

        [ConfigurationProperty(UseMinifiedResourcesAttribute, IsRequired = false, DefaultValue = true)]
        public bool UseMinifiedResources
        {
            get { return Convert.ToBoolean(this[UseMinifiedResourcesAttribute]); }
            set { this[UseMinifiedResourcesAttribute] = value; }
        }

        [ConfigurationProperty(ResourcesBasePathAttribute, IsRequired = false, DefaultValue = null)]
        public string ResourcesBasePath
        {
            get { return Convert.ToString(this[ResourcesBasePathAttribute]); }
            set { this[ResourcesBasePathAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether enforce permissions.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enforce permissions]; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(EnforcePermissionsAttribute, DefaultValue = false, IsRequired = false)]
        public bool EnforcePermissions
        {
            get { return Convert.ToBoolean(this[EnforcePermissionsAttribute]); }
            set { this[EnforcePermissionsAttribute] = value; }
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
        /// Gets or sets the article url prefix.
        /// </summary>
        /// <value>
        /// The article url prefix.
        /// </value>
        [ConfigurationProperty(ArticleUrlPrefixAttribute, IsRequired = false)]
        public string ArticleUrlPrefix {
            get {return Convert.ToString(this[ArticleUrlPrefixAttribute]); }
            set { this[ArticleUrlPrefixAttribute] = value; }
        }

        #endregion

        #region Child Nodes

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

        /// <summary>
        /// Gets or sets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        [ConfigurationProperty(SideMenuSectionsNode, IsRequired = false)]
        public SectionElementCollection Sections
        {
            get { return (SectionElementCollection)this[SideMenuSectionsNode]; }
            set { this[SideMenuSectionsNode] = value; }
        }        

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

        [ConfigurationProperty(ModuleGalleryNode, IsRequired = true)]
        public CmsModuleGalleryConfigurationElement ModuleGallery
        {
            get { return (CmsModuleGalleryConfigurationElement)this[ModuleGalleryNode]; }
            set { this[ModuleGalleryNode] = value; }
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

        #endregion
    }
}
