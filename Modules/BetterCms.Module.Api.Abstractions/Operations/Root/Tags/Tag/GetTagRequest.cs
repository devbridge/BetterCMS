using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Request to get a tag.
    /// </summary>
    [Route("/tags/{TagId}", Verbs = "GET")]
    [Route("/tags/by-name/{TagName}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetTagRequest : IValidatableObject
    {
        /// <summary>
        /// Gets or sets the tag identifier.
        /// </summary>
        /// <value>
        /// The tag identifier.
        /// </value>
        [DataMember]
        public Guid? TagId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <value>
        /// The name of the tag.
        /// </value>
        [DataMember]
        public string TagName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TagId == null && string.IsNullOrEmpty(TagName))
            {
                yield return new ValidationResult("A TagId or TagName should be provided.", new List<string> {"TagName"});
            }

            if (TagId != null && string.IsNullOrEmpty(TagName) || TagId == null && !string.IsNullOrEmpty(TagName)){}
            else
            {
                yield return new ValidationResult("A TagName field must be null if TagId is provided.", new List<string> {"TagId"});
            }
        }
    }
}