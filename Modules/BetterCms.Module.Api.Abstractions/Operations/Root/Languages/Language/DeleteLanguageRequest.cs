using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    /// <summary>
    /// Request for language update or creation.
    /// </summary>
    [Route("/languages/{LanguageId}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteLanguageRequest : DeleteRequestBase, IReturn<DeleteLanguageResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid LanguageId { get; set; }
    }
}