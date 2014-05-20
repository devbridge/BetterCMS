using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    /// <summary>
    /// Request for language update or creation.
    /// </summary>
    [Route("/languages/{Id}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteLanguageRequest : DeleteRequestBase, IReturn<DeleteLanguageResponse>
    {
    }
}