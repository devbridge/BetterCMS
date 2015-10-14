using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    [DataContract]
    [Serializable]
    public class GetPageRequest : IValidatableObject
    {
        [DataMember]
        public Guid? PageId { get; set; }

        [DataMember]
        public string PageUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((PageId != null && string.IsNullOrEmpty(PageUrl)) || (PageId == null && !string.IsNullOrEmpty(PageUrl))){}
            else
            {
                yield return new ValidationResult("A PageUrl field must be null if PageId is provided.", new List<string> { "Data.PageId" });
            }

            if (PageId != null || !string.IsNullOrEmpty(PageUrl)){}
            else
            {
                yield return new ValidationResult("A PageId or PageUrl should be provided.", new List<string> { "Data.PageUrl" });
            }

        }
    }
}