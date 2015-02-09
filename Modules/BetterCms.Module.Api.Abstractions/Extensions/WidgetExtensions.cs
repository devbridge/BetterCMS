using System.Linq;

using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget;

namespace BetterCms.Module.Api.Extensions
{
    public static class WidgetExtensions
    {
        public static PutHtmlContentWidgetRequest ToPutRequest(this GetHtmlContentWidgetResponse response)
        {
            var model = MapHtmlContentWidgetModel(response);

            return new PutHtmlContentWidgetRequest { Data = model, Id = response.Data.Id };
        }

        public static PostHtmlContentWidgetRequest ToPostRequest(this GetHtmlContentWidgetResponse response)
        {
            var model = MapHtmlContentWidgetModel(response);

            return new PostHtmlContentWidgetRequest { Data = model };
        }

        private static SaveHtmlContentWidgetModel MapHtmlContentWidgetModel(GetHtmlContentWidgetResponse response)
        {
            var model = new SaveHtmlContentWidgetModel
                        {
                            Version = response.Data.Version,
                            Name = response.Data.Name,
                            IsPublished = response.Data.IsPublished,
                            PublishedOn = response.Data.PublishedOn,
                            PublishedByUser = response.Data.PublishedByUser,
                            Categories = response.Categories.Select(c => c.Id).ToList(),
                            CustomCss = response.Data.CustomCss,
                            UseCustomCss = response.Data.UseCustomCss,
                            Html = response.Data.Html,
                            UseHtml = response.Data.UseHtml,
                            CustomJavaScript = response.Data.CustomJavaScript,
                            UseCustomJavaScript = response.Data.UseCustomJavaScript,
                            Options = response.Options,
                            ChildContentsOptionValues = response.ChildContentsOptionValues
                        };

            return model;
        }

        public static PutServerControlWidgetRequest ToPutRequest(this GetServerControlWidgetResponse response)
        {
            var model = MapServerControlWidgetModel(response);

            return new PutServerControlWidgetRequest { Data = model, Id = response.Data.Id };
        }

        public static PostServerControlWidgetRequest ToPostRequest(this GetServerControlWidgetResponse response)
        {
            var model = MapServerControlWidgetModel(response);

            return new PostServerControlWidgetRequest { Data = model };
        }

        private static SaveServerControlWidgetModel MapServerControlWidgetModel(GetServerControlWidgetResponse response)
        {
            var model = new SaveServerControlWidgetModel
                        {
                            Version = response.Data.Version,
                            Name = response.Data.Name,
                            IsPublished = response.Data.IsPublished,
                            PublishedOn = response.Data.PublishedOn,
                            PublishedByUser = response.Data.PublishedByUser,
                            Categories = response.Categories.Select(c => c.Id).ToList(),
                            PreviewUrl = response.Data.PreviewUrl,
                            WidgetUrl = response.Data.WidgetUrl,
                            Options = response.Options
                        };

            return model;
        }
    }
}
