using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree
{
    [Route("/sitemaps/{SitemapId}/tree/", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetSitemapTreeRequest : RequestBase<GetSitemapTreeModel>, IValidatableObject
    {
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