using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
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
        public System.Collections.Generic.IList<AccessRuleModel> AccessRules { get; set; }
    }
}
