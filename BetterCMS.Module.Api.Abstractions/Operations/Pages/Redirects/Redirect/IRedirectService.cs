namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    public interface IRedirectService
    {
        GetRedirectResponse Get(GetRedirectRequest request);
    }
}