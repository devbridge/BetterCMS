namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    public interface IRedirectService
    {
        GetRedirectResponse Get(GetRedirectRequest request);
        
        PutRedirectResponse Put(PutRedirectRequest request);
        
        DeleteRedirectResponse Delete(DeleteRedirectRequest request);
    }
}