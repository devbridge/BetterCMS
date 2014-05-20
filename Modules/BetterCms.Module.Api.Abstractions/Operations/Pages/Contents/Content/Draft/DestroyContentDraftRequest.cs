using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.Draft
{
    /// <summary>
    /// Request for content's draft version destroy operation.
    /// </summary>
    [Route("/contents/{Id}/draft", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DestroyContentDraftRequest : DeleteRequestBase, IReturn<DestroyContentDraftResponse>
    {
    }
}