using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Response with sitemap data.
    /// </summary>
    [Serializable]
    [DataContract]
    public class GetSitemapResponse : ResponseBase<SitemapModel>
    {
        /// <summary>
        /// Gets or sets the access rules.
        /// </summary>
        /// <value>
        /// The access rules.
        /// </value>
        [DataMember]
        public IList<AccessRuleModel> AccessRules { get; set; }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        [DataMember]
        public IList<SitemapNodeWithTranslationsModel> Nodes { get; set; }
    }
}
