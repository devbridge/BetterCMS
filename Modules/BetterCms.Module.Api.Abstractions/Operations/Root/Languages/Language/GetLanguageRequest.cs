using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    [DataContract]
    [Serializable]
    public class GetLanguageRequest : IValidatableObject
    {
        [DataMember]
        public Guid? LanguageId { get; set; }

        [DataMember]
        public string LanguageCode { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LanguageId == null && string.IsNullOrEmpty(LanguageCode))
            {
                yield return new ValidationResult("A LanguageId or LanguageCode should be provided.", new List<string> {"LanguageCode"});
            }

            if (LanguageId != null && string.IsNullOrEmpty(LanguageCode) || LanguageId == null && !string.IsNullOrEmpty(LanguageCode)){}
            else
            {
                yield return new ValidationResult("A LanguageCode field must be null if LanguageId is provided.", new List<string> {"LanguageId"});
            }
        }
    }
}