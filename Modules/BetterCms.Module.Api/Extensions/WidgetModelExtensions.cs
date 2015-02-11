using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget;

using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Api.Extensions
{
    public static class WidgetModelExtensions
    {
        public static EditHtmlContentWidgetViewModel ToServiceModel(this SaveHtmlContentWidgetModel model)
        {
            var serviceModel = new EditHtmlContentWidgetViewModel();

            serviceModel.Version = model.Version;
            serviceModel.Name = model.Name;
            serviceModel.DesirableStatus = model.IsPublished ? ContentStatus.Published : ContentStatus.Draft;
            serviceModel.PublishedOn = model.PublishedOn;
            serviceModel.PublishedByUser = model.PublishedByUser;
            serviceModel.Categories = model.Categories != null ? model.Categories.Select(c => new LookupKeyValue()
            {
                Key = c.ToString(),
            }).ToList() : new List<LookupKeyValue>();

            serviceModel.CustomCSS = model.CustomCss;
            serviceModel.EnableCustomCSS = model.UseCustomCss;
            serviceModel.PageContent = model.Html;
            serviceModel.EnableCustomHtml = model.UseHtml;
            serviceModel.CustomJS = model.CustomJavaScript;
            serviceModel.EnableCustomJS = model.UseCustomJavaScript;

            if (model.Options != null)
            {
                serviceModel.Options = model.Options.ToServiceModel();
            }

            return serviceModel;
        }
        
        public static EditServerControlWidgetViewModel ToServiceModel(this SaveServerControlWidgetModel model)
        {
            var serviceModel = new EditServerControlWidgetViewModel();

            serviceModel.Version = model.Version;
            serviceModel.Name = model.Name;
            serviceModel.DesirableStatus = model.IsPublished ? ContentStatus.Published : ContentStatus.Draft;
            serviceModel.PublishedOn = model.PublishedOn;
            serviceModel.PublishedByUser = model.PublishedByUser;
            serviceModel.Categories = model.Categories != null ? model.Categories.Select(c => new LookupKeyValue()
            {
                Key = c.ToString(),                
            }).ToList() : new List<LookupKeyValue>();
            serviceModel.Url = model.WidgetUrl;
            serviceModel.PreviewImageUrl = model.PreviewUrl;

            if (model.Options != null)
            {
                serviceModel.Options = model.Options.ToServiceModel();
            }

            return serviceModel;
        }
    }
}