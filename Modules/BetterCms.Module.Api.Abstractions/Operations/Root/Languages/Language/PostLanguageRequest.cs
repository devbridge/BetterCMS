using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    /// <summary>
    /// Request for language update or creation.
    /// </summary>
    [Route("/languages", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostLanguageRequest : RequestBase<SaveLanguageModel>, IReturn<PostLanguageResponse>
    {
    }
}