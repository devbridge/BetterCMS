using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Tree
{
    [Route("/categorytrees/{CategoryTreeId}/tree/", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetNodesTreeRequest : RequestBase<GetNodesTreeModel>, IValidatableObject
    {
        [DataMember]
        public Guid CategoryTreeId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CategoryTreeId == Guid.Empty)
            {
                yield return new ValidationResult("A CategoryTreeId field must be provided.", new List<string> {"CategoryTreeId"});
            }
        }
    }
}