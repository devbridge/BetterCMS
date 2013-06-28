using System.Linq;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Api.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    public class LayoutsService : Service, ILayoutsService
    {
        public GetLayoutsResponse Get(GetLayoutsRequest request)
        {
            var request2 = new Module.Pages.Api.DataContracts.GetLayoutsRequest();
            request.ApplyTo(request2);

            using (var api = CmsContext.CreateApiContextOf<PagesApiContext>())
            {
                var layouts = api.GetLayouts(request2);

                return new GetLayoutsResponse
                {
                    Data = new DataListResponse<LayoutModel>(
                        layouts.Items.Select(
                            f => new LayoutModel
                            {
                                Id = f.Id,
                                Name = f.Name,
                                IsDeleted = f.IsDeleted,
                                Version = f.Version,
                                PreviewUrl = f.PreviewUrl,
                                LayoutPath = f.LayoutPath
                            }).ToList(),
                        layouts.TotalCount)
                };
            }
        }
    }
}