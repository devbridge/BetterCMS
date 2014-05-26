using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node
{
    /// <summary>
    /// Sitemap node delete response.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeleteNodeResponse : DeleteResponseBase
    {
    }
}