using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree
{
    [Serializable]
    [DataContract]
    public class GetSitemapTreeResponse : ResponseBase<System.Collections.Generic.List<SitemapTreeNodeModel>>
    {
    }
}