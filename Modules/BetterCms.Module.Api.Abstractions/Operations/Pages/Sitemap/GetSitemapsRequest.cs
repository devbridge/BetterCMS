using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    [Route("/sitemap-trees", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetSitemapsRequest : RequestBase<GetSitemapsModel>, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Data.HasColumnInSortBySection("Tags") || Data.HasColumnInWhereSection("Tags"))
            {
                yield return new ValidationResult("An Tags field is a list. You can't sort or add filter by this column.", new List<string> {"Data"});
            }
        }
    }
}