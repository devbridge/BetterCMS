using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    public class RedirectService : Service, IRedirectService
    {
        public GetRedirectResponse Get(GetRedirectRequest request)
        {
            // TODO: need implementation
            return new GetRedirectResponse
                       {
                           Data = new RedirectModel
                                     {
                                         Id = request.RedirectId
                                     }
                       };
        }
    }
}