using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Data model for sitemap save.
    /// </summary>
    public class SaveSitemapModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the access rules.
        /// </summary>
        /// <value>
        /// The access rules.
        /// </value>
        [DataMember]
        public IList<AccessRuleModel> AccessRules { get; set; }
    }
}
