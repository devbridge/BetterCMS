using BetterCms.Module.Api.Operations.Pages.Redirects;
using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;

namespace BetterCms.Module.Api.Extensions
{
    public static class RedirectExtensions
    {
        public static PostRedirectRequest ToPostRequest(this GetRedirectResponse response)
        {
            var model = MapModel(response);

            return new PostRedirectRequest { Data = model };
        }

        public static PutRedirectRequest ToPutRequest(this GetRedirectResponse response)
        {
            var model = MapModel(response);

            return new PutRedirectRequest { Data = model, Id = response.Data.Id };
        }

        private static SaveRedirectModel MapModel(GetRedirectResponse response)
        {
            var model = new SaveRedirectModel
                        {
                            Version = response.Data.Version,
                            PageUrl = response.Data.PageUrl,
                            RedirectUrl = response.Data.RedirectUrl,
                        };

            return model;
        }
    }
}
