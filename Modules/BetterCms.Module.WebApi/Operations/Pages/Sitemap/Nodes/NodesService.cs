using System.Collections.Generic;

using BetterCms.Core.Api.DataContracts;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes
{
    public class NodesService : Service, INodesService
    {
        public GetSitemapNodesResponse Get(GetSitemapNodesRequest request)
        {
            // TODO: need implementation
            return new GetSitemapNodesResponse
                       {
                           Data = new DataListResponse<SitemapNodeModel>
                                      {
                                          TotalCount = 256,
                                          Items = new List<SitemapNodeModel>
                                                      {
                                                          new SitemapNodeModel(),
                                                          new SitemapNodeModel(),
                                                          new SitemapNodeModel()
                                                      }
                                      }
                       };
        }
    }
}