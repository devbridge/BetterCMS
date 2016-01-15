// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICmsConfiguration.cs" company="Devbridge Group LLC">
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
using BetterCms.Configuration;

using BetterModules.Core.Web.Configuration;

namespace BetterCms
{
    /// <summary>
    /// </summary>
    public interface ICmsConfiguration : IWebConfiguration
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
        /// Gets the configuration of CMS permissions service.
        /// </summary>
        ICmsSecurityConfiguration Security { get;  }

        /// <summary>
        /// Gets the configuration of CMS users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        ICmsUsersConfiguration Users { get; }

        /// <summary>
        /// Gets the configuration of CMS search services.
        /// </summary>
        ICmsSearchConfiguration Search { get; }

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
        /// Gets or sets the modules.
        /// </summary>
        /// <value> The modules. </value>
        ModulesCollection Modules { get; set; }

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
        /// Gets a value indicating whether to render content ending div.
        /// </summary>
        /// <value>
        /// <c>true</c> if to render content ending div; otherwise, <c>false</c>.
        /// </value>
        bool RenderContentEndingDiv { get; }

        /// <summary>
        /// Gets the name of the content ending div CSS class.
        /// </summary>
        /// <value>
        /// The name of the content ending div CSS class.
        /// </value>
        string ContentEndingDivCssClassName { get; }

        /// <summary>
        /// Gets a value indicating whether to enable multilanguage.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to enable multilanguage; otherwise, <c>false</c>.
        /// </value>
        bool EnableMultilanguage { get; }

        /// <summary>
        /// Gets a value indicating whether macros are enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if macros are enabled; otherwise, <c>false</c>.
        /// </value>
        bool EnableMacros { get; }
    }
}