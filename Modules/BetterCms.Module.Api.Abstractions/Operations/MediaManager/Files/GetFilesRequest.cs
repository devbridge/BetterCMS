using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    [DataContract]
    [System.Serializable]
    public class GetFilesRequest : RequestBase<GetFilesModel>, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Data.IncludeFiles && !Data.IncludeFolders)
            {
                yield return new ValidationResult("At least one of: IncludeFolders and IncludeImages should be provided.", new List<string> { "Data.IncludeFolders" });
            }
        }
    }
}
