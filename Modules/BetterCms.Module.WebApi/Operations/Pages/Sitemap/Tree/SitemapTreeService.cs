using System.Collections.Generic;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    public class SitemapTreeService : Service, ISitemapTreeService
    {
        public GetSitemapTreeResponse Get(GetSitemapTreeRequest request)
        {
            // TODO: need implementation
            return new GetSitemapTreeResponse
                       {
                           Data = new List<SitemapTreeNodeModel>
                                      {
                                          new SitemapTreeNodeModel(),
                                          new SitemapTreeNodeModel(),
                                          new SitemapTreeNodeModel()
                                      }
                       };
        }
    }
}