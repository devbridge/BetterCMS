using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents
{
    [Route("/pages/{PageId}/contents")]
    [DataContract]
    [Serializable]
    public class GetPageContentsRequest : RequestBase<GetPageContentsModel>, IReturn<GetPageContentsResponse>, IValidatableObject
    {
        [DataMember]
        public Guid PageId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Data.HasColumnInSortBySection("ContentType") || Data.HasColumnInWhereSection("ContentType"))
            {
                yield return new ValidationResult("A ContentType field is a dynamically calculated field. You can't sort or add filter by this column.", new List<string> {"Data"});
            }

            if ((!string.IsNullOrWhiteSpace(Data.RegionIdentifier) && Data.RegionId.HasValue))
            {
                yield return new ValidationResult("A RegionIdentifier field must be null if RegionId field is provided.", new List<string> {"Data.RegionIdentifier"});
            }
        }
    }
}