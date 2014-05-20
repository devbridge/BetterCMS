using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/languages/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutLanguageRequest : PutRequestBase<SaveLanguageModel>, IReturn<PutLanguageResponse>
    {
    }
}