namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists
{
    public interface IPageExistsService
    {
        PageExistsResponse Get(PageExistsRequest request);
    }
}