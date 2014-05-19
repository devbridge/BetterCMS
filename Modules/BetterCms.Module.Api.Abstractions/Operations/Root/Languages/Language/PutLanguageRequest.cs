using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/languages/{LanguageId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutLanguageRequest : RequestBase<SaveLanguageModel>, IReturn<PutLanguageResponse>
    {
        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        /// <value>
        /// The language identifier.
        /// </value>
        [DataMember]
        public Guid? LanguageId { get; set; }
    }
}