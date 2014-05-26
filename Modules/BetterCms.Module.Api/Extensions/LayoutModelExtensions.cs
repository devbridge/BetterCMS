using System.Linq;

using BetterCms.Module.Api.Operations.Root.Layouts.Layout;

using BetterCms.Module.Pages.ViewModels.Templates;

namespace BetterCms.Module.Api.Extensions
{
    public static class LayoutModelExtensions
    {
        public static TemplateEditViewModel ToServiceModel(this SaveLayoutModel model)
        {
            var serviceModel = new TemplateEditViewModel();

            serviceModel.Version = model.Version;
            serviceModel.Name = model.Name;
            serviceModel.Url = model.LayoutPath;
            serviceModel.PreviewImageUrl = model.PreviewUrl;

            if (model.Options != null)
            {
                serviceModel.Options = model.Options.ToServiceModel();
            }

            if (model.Regions != null)
            {
                serviceModel.Regions = model
                    .Regions
                    .Select(r => new TemplateRegionItemViewModel
                                 {
                                     Description = r.Description,
                                     Identifier = r.RegionIdentifier
                                 })
                    .ToList();
            }

            return serviceModel;
        }
    }
}