using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations
{
    [Route("/pages/{PageId}/translations")]
    [Route("/pages/translations/by-url/{PageUrl}")]
    [DataContract]
    [Serializable]
    public class GetPageTranslationsRequest : RequestBase<DataOptions>, IValidatableObject
    {
        [DataMember]
        public Guid? PageId { get; set; }
        
        [DataMember]
        public string PageUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageId != null && string.IsNullOrEmpty(PageUrl) || PageId == null && !string.IsNullOrEmpty(PageUrl)) { }
            else
            {
                yield return new ValidationResult("A PageUrl field must be null if PageId is provided.", new List<string> {"PageId"});
            }

            if (PageId != null || !string.IsNullOrEmpty(PageUrl)) { }
            else
            {
                yield return new ValidationResult("A PageId or PageUrl should be provided.", new List<string> {"PageUrl"});
            }
        }
    }
}