using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    [DataContract]
    [Serializable]
    public class GetSitemapTreeResponse : ResponseBase<System.Collections.Generic.List<SitemapTreeNodeModel>>
    {
    }
}