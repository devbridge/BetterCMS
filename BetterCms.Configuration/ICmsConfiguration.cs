using System;

using BetterCms.Configuration;

namespace BetterCms
{
    /// <summary>
    /// </summary>
    public interface ICmsConfiguration
    {
        /// <summary>
        /// Gets the Better CMS version.
        /// </summary>
        /// <value>
        /// The Better CMS version.
        /// </value>
        string Version { get; }

        /// <summary>
        /// Gets a value indicating whether CMS should use minified resources (*.min.js and *.min.css).
        /// </summary>
        /// <value>
        ///   <c>true</c> if CMS should use minified resources; otherwise, <c>false</c>.
        /// </value>
        bool UseMinifiedResources { get; }

        /// <summary>
        /// Gets the CMS resources (*.js and *.css) base path.
        /// </summary>
        /// <value>
        /// The CMS content base path.
        /// </value>
        string ResourcesBasePath { get; }

        /// <summary>
        /// Gets or sets the login URL.
        /// </summary>
        /// <value>
        /// The login URL.
        /// </value>
        string LoginUrl { get; set; }

        /// <summary>
        /// Gets the virtual root path (like "~/App_Data") of BetterCMS working directory. 
        /// </summary>
        /// <value>
        /// The virtual root path of BetterCMS working directory.
        /// </value>
        string WorkingDirectoryRootPath { get; }

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
        string ArticleUrlPattern { get; set; }

        /// <summary>
        /// Gets or sets the URL patterns.
        /// </summary>
        /// <value> The URL patterns. </value>
        UrlPatternsCollection UrlPatterns { get; set; }

        /// <summary>
        /// Gets the url of nuget feed for BetterCms modules.
        /// </summary>
        ICmsModuleGalleryConfiguration ModuleGallery { get; }

        /// <summary>
        /// Gets the installation configuration.
        /// </summary>
        ICmsInstallationConfiguration Installation { get; }

        /// <summary>
        /// Gets the cache configuration.
        /// </summary>
        ICmsCacheConfiguration Cache { get; }

        /// <summary>
        /// Gets the URL mode.
        /// </summary>
        TrailingSlashBehaviorType UrlMode { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [access control enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [access control enabled]; otherwise, <c>false</c>.
        /// </value>
        bool AccessControlEnabled { get; set; }
    }
}