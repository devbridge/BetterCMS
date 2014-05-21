using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node
{
    /// <summary>
    /// Page sitemap node response.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PutNodeResponse : SaveResponseBase
    {
    }
}