using BetterCms.Module.Api.Operations.Root.Layouts.Layout;

using BetterCms.Module.Pages.ViewModels.Templates;

namespace BetterCms.Module.Api.Extensions
{
    public static class LayoutModelExtensions
    {
        public static TemplateEditViewModel ToServiceModel(this LayoutSaveModel model)
        {
            var serviceModel = new TemplateEditViewModel();

            serviceModel.Version = model.Version;
            serviceModel.Name = model.Name;
            serviceModel.Url = model.LayoutPath;
            serviceModel.PreviewImageUrl = model.PreviewUrl;

            return serviceModel;
        }
    }
}