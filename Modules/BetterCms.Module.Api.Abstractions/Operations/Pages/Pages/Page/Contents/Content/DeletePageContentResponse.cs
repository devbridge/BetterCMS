using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    /// <summary>
    /// Response for image delete operation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeletePageContentResponse : DeleteResponseBase
    {
    }
}
