using System;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    public class PagePropertiesService : Service, IPagePropertiesService
    {
        public GetPagePropertiesResponse Get(GetPagePropertiesRequest request)
        {
            return new GetPagePropertiesResponse
                       {
                           Data = new PagePropertiesModel
                                      {
                                          Id = request.PageId.HasValue ? request.PageId.Value : new Guid(),
                                          PageUrl = request.PageUrl
                                      },
                           Layout = new LayoutModel(),
                           Category = new CategoryModel(),
                           MainImage = new ImageModel(),
                           FeaturedImage = new ImageModel(),
                           SecondaryImage = new ImageModel()
                       };
        }
    }
}