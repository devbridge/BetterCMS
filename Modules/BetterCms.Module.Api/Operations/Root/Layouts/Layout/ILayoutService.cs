using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    public interface ILayoutService
    {
        GetLayoutResponse Get(GetLayoutRequest request);

        ILayoutRegionService Regions { get; }
    }
}
