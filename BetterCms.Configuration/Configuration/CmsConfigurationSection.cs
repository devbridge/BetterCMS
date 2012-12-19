using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class CmsConfigurationSection : ConfigurationSection, ICmsConfiguration
    {
        private const string LoginUrlAttribute = "loginUrl";
        private const string ResourcesRootAttribute = "resourcesRoot";
        private const string ControlerPathAttribute = "controlerPath";
        private const string LocalresourcesPathAttribute = "localResourcesPath";
        private const string DevelopmentEnvironmentAttribute = "cmsDevEnv";
        private const string DefaultImageWidthAttribute = "defaultImageWidth";
        private const string EnforcePermissionsAttribute = "enforcePermissions";
        private const string PageCheckoutEnabledAttribute = "pageCheckoutEnabled";
        private const string PageNotFoundUrlAttribute = "pageNotFoundUrl";
        private const string SideMenuSectionsNode = "menuSections";
        private const string UrlPatternsNode = "urlPatterns";
        private const string DatabaseNode = "database";
        private const string StorageNode = "storage";
        private const string CacheNode = "cache";
        private const string SecurityNode = "security";
        private const string ModuleGalleryNode = "moduleGallery";
        private const string WorkingDirectoryRootPathAttribute = "workingDirectoryRootPath";
        
        #region Attributes

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
        /// Gets or sets the resources root.
        /// </summary>
        /// <value>
        /// The resources root.
        /// </value>
        [ConfigurationProperty(ResourcesRootAttribute, DefaultValue = "http://dbcms.s3.amazonaws.com/cms/", IsRequired = false)]
        public string ResourcesRoot
        {
            get { return Convert.ToString(this[ResourcesRootAttribute]); }
            set { this[ResourcesRootAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the controller path.
        /// </summary>
        /// <value>
        /// The controller path.
        /// </value>
        [ConfigurationProperty(ControlerPathAttribute, DefaultValue = "/cms/", IsRequired = false)]
        public string ControlerPath
        {
            get { return Convert.ToString(this[ControlerPathAttribute]); }
            set { this[ControlerPathAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the local resources path.
        /// </summary>
        /// <value>
        /// The local resources path.
        /// </value>
        [ConfigurationProperty(LocalresourcesPathAttribute, DefaultValue = "/cms/local/", IsRequired = false)]
        public string LocalResourcesPath
        {
            get { return Convert.ToString(this[LocalresourcesPathAttribute]); }
            set { this[LocalresourcesPathAttribute] = value; }
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
        /// Gets or sets a value indicating whether [CMS developer environment].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [CMS dev env]; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(DevelopmentEnvironmentAttribute, DefaultValue = false, IsRequired = false)]
        public bool CmsDevEnv
        {
            get { return Convert.ToBoolean(this[DevelopmentEnvironmentAttribute]); }
            set { this[DevelopmentEnvironmentAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the default width of the image.
        /// </summary>
        /// <value>
        /// The default width of the image.
        /// </value>
        [ConfigurationProperty(DefaultImageWidthAttribute, DefaultValue = 550, IsRequired = false)]
        public int DefaultImageWidth
        {
            get { return Convert.ToInt32(this[DefaultImageWidthAttribute]); }
            set { this[DefaultImageWidthAttribute] = value; }
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
        /// Gets or sets a value indicating whether page checkout is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if page checkout enable; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(PageCheckoutEnabledAttribute, DefaultValue = false, IsRequired = false)]
        public bool PageCheckoutEnabled
        {
            get { return Convert.ToBoolean(this[PageCheckoutEnabledAttribute]); }
            set { this[PageCheckoutEnabledAttribute] = value; }
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
