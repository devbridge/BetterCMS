using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node
{
    [Serializable]
    [DataContract]
    public class GetNodeResponse : ResponseBase<NodeModel>
    {
        /// <summary>
        /// Gets or sets the translations.
        /// </summary>
        /// <value>
        /// The translations.
        /// </value>
        [DataMember]
        public IList<NodeTranslationModel> Translations { get; set; }
    }
}