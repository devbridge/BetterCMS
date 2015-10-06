using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes
{
    /// <summary>
    /// Request to get sitemap data.
    /// </summary>
    [Serializable]
    [DataContract]
    public class GetSitemapNodesRequest : RequestBase<DataOptions>, IValidatableObject
    {
        /// <summary>
        /// Gets or sets the sitemap identifier.
        /// </summary>
        /// <value>
        /// The sitemap identifier.
        /// </value>
        [DataMember]
        public Guid SitemapId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SitemapId == Guid.Empty)
            {
                yield return new ValidationResult("A SitemapId field must be provided.", new List<string> {"SitemapId"});
            }
        }
    }
}