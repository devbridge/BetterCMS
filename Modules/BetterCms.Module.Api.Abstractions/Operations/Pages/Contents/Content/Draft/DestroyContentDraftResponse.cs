using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.Draft
{
    /// <summary>
    /// Response for content draft destroy operation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DestroyContentDraftResponse : DeleteResponseBase
    {
    }
}