using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    [Route("/page-properties/{PageId}", Verbs = "GET")]
    [Route("/page-properties/by-url/{PageUrl*}", Verbs = "GET")]
    [DataContract]
    [System.Serializable]
    public class GetPagePropertiesRequest : RequestBase<GetPagePropertiesModel>, IReturn<GetPagePropertiesResponse>, IValidatableObject
    {
        [DataMember]
        public System.Guid? PageId { get; set; }

        [DataMember]
        public string PageUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((PageId != null && string.IsNullOrEmpty(PageUrl)) || (PageId == null && !string.IsNullOrEmpty(PageUrl))){}
            else {
                yield return new ValidationResult("A PageUrl field must be null if PageId is provided.", new List<string> {"Data.PageId"});
            }

            if (PageId != null || !string.IsNullOrEmpty(PageUrl)){}
            else {
                yield return new ValidationResult("A PageId or PageUrl should be provided.", new List<string> { "Data.PageUrl" });
            }
            
        }
    }
}