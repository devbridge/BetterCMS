using BetterCms.Configuration;

namespace BetterCms
{
    /// <summary>
    /// </summary>
    public interface ICmsConfiguration
    {
        /// <summary>
        /// Gets or sets the login URL.
        /// </summary>
        /// <value>
        /// The login URL.
        /// </value>
        string LoginUrl { get; set; }

        /// <summary>
        /// Gets or sets the resources root.
        /// </summary>
        /// <value>
        /// The resources root.
        /// </value>
        string ResourcesRoot { get; set; }

        /// <summary>
        /// Gets or sets the controller path.
        /// </summary>
        /// <value>
        /// The controller path.
        /// </value>
        string ControlerPath { get; set; }

        /// <summary>
        /// Gets or sets the local resources path.
        /// </summary>
        /// <value>
        /// The local resources path.
        /// </value>
        string LocalResourcesPath { get; set; }

        /// <summary>
        /// Gets the virtual root path (like "~/App_Data") of BetterCMS working directory. 
        /// </summary>
        /// <value>
        /// The virtual root path of BetterCMS working directory.
        /// </value>
        string WorkingDirectoryRootPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [CMS developer environment].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [CMS dev env]; otherwise, <c>false</c>.
        /// </value>
        bool CmsDevEnv { get; set; }

        /// <summary>
        /// Gets or sets the default width of the image.
        /// </summary>
        /// <value>
        /// The default width of the image.
        /// </value>
        int DefaultImageWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enforce permissions.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enforce permissions]; otherwise, <c>false</c>.
        /// </value>
        bool EnforcePermissions { get; set; }

        /// <summary>
        /// Gets or sets the URL patterns.
        /// </summary>
        /// <value>
        /// The URL patterns.
        /// </value>
        UrlPatternsCollection UrlPatterns { get; set; }

        /// <summary>
        /// Gets or sets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        SectionElementCollection Sections { get; set; }

        /// <summary>
        /// Gets the configuration of CMS storage service.
        /// </summary>
        /// <value>
        /// The storage service.
        /// </value>
        ICmsStorageConfiguration Storage { get; }

        /// <summary>
        /// Gets the configuration of CMS database.
        /// </summary>
        ICmsDatabaseConfiguration Database { get; }

        /// <summary>
        /// Gets the configuration of CMS permissions service.
        /// </summary>
        ICmsSecurityConfiguration Security { get;  }

        /// <summary>
        /// Gets or sets a value indicating whether page checkout enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if page checkout enabled; otherwise, <c>false</c>.
        /// </value>
        bool PageCheckoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the page not found url.
        /// </summary>
        /// <value>
        /// The page not found url.
        /// </value>
        string PageNotFoundUrl { get; set; }

        /// <summary>
        /// Gets or sets the article url prefix.
        /// </summary>
        /// <value>
        /// The article url prefix.
        /// </value>
        string ArticleUrlPrefix { get; set; }

        /// <summary>
        /// Gets the url of nuget feed for BetterCms modules.
        /// </summary>
        ICmsModuleGalleryConfiguration ModuleGallery { get; }

        /// <summary>
        /// Gets the cache configuration.
        /// </summary>
        ICmsCacheConfiguration Cache { get; }
    }
}