using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;
using BetterCms.Module.Pages.ViewModels.SiteSettings;

namespace BetterCms.Module.Api.Extensions
{
    public static class RedirectModelExtensions
    {
        public static SiteSettingRedirectViewModel ToServiceModel(this SaveRedirectModel model)
        {
            var serviceModel = new SiteSettingRedirectViewModel();

            serviceModel.Version = model.Version;
            serviceModel.PageUrl = model.PageUrl;
            serviceModel.RedirectUrl = model.RedirectUrl;
            
            return serviceModel;
        }
    }
}