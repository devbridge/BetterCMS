// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Page.cs" company="Devbridge Group LLC">
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
using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Security;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    /// <summary>
    /// A generic page entity.
    /// </summary>
    [Serializable]
    public class Page : EquatableEntity<Page>, IPage, IAccessSecuredObject
    {
        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        public virtual string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the lower trimmed page URL.
        /// </summary>
        /// <value>
        /// The lower trimmed page URL.
        /// </value>
        public virtual string PageUrlHash { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the page status.
        /// </summary>
        /// <value>
        /// The page status.
        /// </value>
        public virtual PageStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the page published date.
        /// </summary>
        /// <value>
        /// The page published date.
        /// </value>
        public virtual DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets a value indicating whether this page has SEO meta data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this page has SEO; otherwise, <c>false</c>.
        /// </value>
        public virtual bool HasSEO
        {
            get
            {
                return !string.IsNullOrWhiteSpace(MetaTitle)
                    && !string.IsNullOrWhiteSpace(MetaKeywords)
                    && !string.IsNullOrWhiteSpace(MetaDescription);
            }
        }

        /// <summary>
        /// Gets or sets the page meta title.
        /// </summary>
        /// <value>
        /// The page meta title.
        /// </value>
        public virtual string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the page meta keywords.
        /// </summary>
        /// <value>
        /// The page meta keywords.
        /// </value>
        public virtual string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the page meta description.
        /// </summary>
        /// <value>
        /// The page meta description.
        /// </value>
        public virtual string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether page can be a master page.
        /// </summary>
        /// <value>
        /// <c>true</c> if page can be a master page; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsMasterPage { get; set; }

        /// <summary>
        /// Gets or sets the type of the access (http vs https).
        /// </summary>
        /// <value>
        /// The type of the access (http vs https).
        /// </value>
        public virtual ForceProtocolType ForceAccessProtocol { get; set; }

        /// <summary>
        /// Gets or sets the page layout.
        /// </summary>
        /// <value>
        /// The page layout.
        /// </value>
        public virtual Layout Layout { get; set; }
        
        /// <summary>
        /// Gets or sets the master page.
        /// </summary>
        /// <value>
        /// The master page.
        /// </value>
        public virtual Page MasterPage { get; set; }

        /// <summary>
        /// Gets or sets the page language.
        /// </summary>
        /// <value>
        /// The page language.
        /// </value>
        public virtual Language Language { get; set; }

        /// <summary>
        /// Gets or sets the language group identifier.
        /// </summary>
        /// <value>
        /// The language group identifier.
        /// </value>
        public virtual Guid? LanguageGroupIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the page contents.
        /// </summary>
        /// <value>
        /// The page contents.
        /// </value>
        public virtual IList<PageContent> PageContents { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public virtual IList<PageOption> Options { get; set; }

        /// <summary>
        /// Gets or sets the master pages.
        /// </summary>
        /// <value>
        /// The master pages.
        /// </value>
        public virtual IList<MasterPage> MasterPages { get; set; }

        /// <summary>
        /// Gets or sets the access rules.
        /// </summary>
        /// <value>
        /// The access rules.
        /// </value>
        public virtual IList<AccessRule> AccessRules { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        /// <value>
        /// The rules.
        /// </value>
        IList<IAccessRule> IAccessSecuredObject.AccessRules
        {
            get
            {
                if (AccessRules == null)
                {
                    return null;
                }

                return AccessRules.Cast<IAccessRule>().ToList();
            }           
        }

        public virtual bool SaveUnsecured { get; set; }

        public virtual PagesView PagesView { get; set; }

        public virtual void AddRule(IAccessRule accessRule)
        {
            if (AccessRules == null)
            {
                AccessRules = new List<AccessRule>();
            }

            AccessRules.Add((AccessRule)accessRule);
        }

        public virtual void RemoveRule(IAccessRule accessRule)
        {            
            AccessRules.Remove((AccessRule)accessRule);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Title={1}", base.ToString(), Title);
        }
    }
}