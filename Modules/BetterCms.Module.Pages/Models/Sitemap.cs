using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Security;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models
{
    /// <summary>
    /// Site map entity.
    /// </summary>
    [Serializable]
    public class Sitemap : EquatableEntity<Sitemap>, IAccessSecuredObject
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the sitemap tags.
        /// </summary>
        /// <value>
        /// The sitemap tags.
        /// </value>
        public virtual IList<SitemapTag> SitemapTags { get; set; }

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
                return AccessRules == null ? null : AccessRules.Cast<IAccessRule>().ToList();
            }
        }

        /// <summary>
        /// Adds the rule.
        /// </summary>
        /// <param name="accessRule">The access rule.</param>
        public virtual void AddRule(IAccessRule accessRule)
        {
            if (AccessRules == null)
            {
                AccessRules = new List<AccessRule>();
            }

            AccessRules.Add((AccessRule)accessRule);
        }

        /// <summary>
        /// Removes the rule.
        /// </summary>
        /// <param name="accessRule">The access rule.</param>
        public virtual void RemoveRule(IAccessRule accessRule)
        {
            AccessRules.Remove((AccessRule)accessRule);
        }

        /// <summary>
        /// Gets a value indicating whether entity should be saved without checking object security.
        /// </summary>
        /// <value>
        /// <c>true</c> if entity can be saved unsecured; otherwise, <c>false</c>.
        /// </value>
        public virtual bool SaveUnsecured { get; set; }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public virtual IList<SitemapNode> Nodes { get; set; }
    }
}