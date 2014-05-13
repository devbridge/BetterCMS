using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Options;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    public interface ILayoutService
    {
        GetLayoutResponse Get(GetLayoutRequest request);
        
        PutLayoutResponse Put(PutLayoutRequest request);

        DeleteLayoutResponse Delete(DeleteLayoutRequest request);

        ILayoutRegionsService Regions { get; }
        
        ILayoutOptionsService Options { get; }
    }
}
