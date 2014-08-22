using System.Linq;

using BetterCms.Module.Api.Operations.Root.Layouts;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions;

namespace BetterCms.Module.Api.Extensions
{
    public static class LayoutExtensions
    {
        public static PutLayoutRequest ToPutRequest(this GetLayoutResponse response)
        {
            var model = MapPageModel(response);

            return new PutLayoutRequest { Data = model, Id = response.Data.Id };
        }

        public static PostLayoutRequest ToPostRequest(this GetLayoutResponse response)
        {
            var model = MapPageModel(response);

            return new PostLayoutRequest { Data = model };
        }

        private static SaveLayoutModel MapPageModel(GetLayoutResponse response)
        {
            var model = new SaveLayoutModel
                {
                    Version = response.Data.Version,
                    Name = response.Data.Name,
                    LayoutPath = response.Data.LayoutPath,
                    PreviewUrl = response.Data.PreviewUrl,
                    Options = response.Options,
                };

            if (response.Regions != null)
            {
                model.Regions = response
                    .Regions
                    .Select(r => new RegionSaveModel
                            {
                                RegionIdentifier = r.RegionIdentifier,
                                Description = r.Description
                            })
                    .ToList();
            }

            return model;
        }
    }
}
