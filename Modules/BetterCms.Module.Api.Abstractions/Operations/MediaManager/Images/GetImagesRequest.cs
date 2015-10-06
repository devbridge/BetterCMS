using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    [DataContract]
    [Serializable]
    public class GetImagesRequest : RequestBase<GetImagesModel>, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Data.IncludeImages && !Data.IncludeFolders)
            {
                yield return new ValidationResult("At least one of: IncludeFolders and IncludeImages should be provided.", new List<string> {"Data.IncludeFolders"});
            }
        }
    }
}
