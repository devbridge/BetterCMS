using System.Linq;

using BetterCms.Api;
using BetterCms.Core;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    public class LayoutService : Service
    {
        public GetLayoutResponse Get(GetLayoutRequest request)
        {
            using (var api = CmsContext.CreateApiContextOf<PagesApiContext>())
            {
                var layout = api
                    .GetLayouts(new Module.Pages.Api.DataContracts.GetLayoutsRequest(l => l.Id == request.LayoutId))
                    .Items
                    .First();
                return new GetLayoutResponse
                {
                    Status = "ok",
                    Data = new LayoutModel
                               {
                                   Id = layout.Id,
                                   Version = layout.Version,
                                   CreatedBy = layout.CreatedByUser,
                                   CreatedOn = layout.CreatedOn,
                                   LastModifiedBy = layout.ModifiedByUser,
                                   LastModifiedOn = layout.ModifiedOn,
                                   PreviewUrl = layout.PreviewUrl,
                                   Name = layout.Name,
                                   LayoutPath = layout.LayoutPath,
                               }
                };
            }
        }
//
//        public PostTagResponse Post(PostTagRequest request)
//        {
//            return new PostTagResponse
//            {
//                Data = request.Id,
//                Status = "ok"
//            };
//        }   
    }
}