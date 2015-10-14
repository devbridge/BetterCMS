using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.Draft
{
    /// <summary>
    /// Request for content's draft version destroy operation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DestroyContentDraftRequest : DeleteRequestBase
    {
    }
}