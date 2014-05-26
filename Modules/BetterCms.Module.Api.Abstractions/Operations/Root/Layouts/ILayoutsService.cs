namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    public interface ILayoutsService
    {
        GetLayoutsResponse Get(GetLayoutsRequest request);
        
        PostLayoutResponse Post(PostLayoutRequest request);
    }
}
