using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    [DataContract]
    [Serializable]
    public class GetPagesRequest : RequestBase<GetPagesModel>, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Data.HasColumnInSortBySection("Options") || Data.HasColumnInWhereSection("Options"))
            {
                yield return new ValidationResult("An Options field is a list. You can't sort or add filter by this column.", new List<string> { "Data" });
            }

            if (Data.HasColumnInSortBySection("Tags") || Data.HasColumnInWhereSection("Tags"))
            {
                yield return new ValidationResult("An Tags field is a list. You can't sort or add filter by this column.", new List<string> {"Data"});
            }
        }
    }
}