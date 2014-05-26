using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    /// <summary>
    /// Response after page content saving.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutPageContentResponse : SaveResponseBase
    {
    }
}
