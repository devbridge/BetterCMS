using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget;

namespace BetterCms.Module.Api.Extensions.Widget
{
    public static class WidgetExtensions
    {
        public static PutHtmlContentWidgetRequest ToPutRequest(this GetHtmlContentWidgetResponse response)
        {
            var model = MapHtmlContentWidgetModel(response);

            return new PutHtmlContentWidgetRequest { Data = model, WidgetId = response.Data.Id };
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
                            CategoryId = response.Data.CategoryId,
                            CustomCss = response.Data.CustomCss,
                            UseCustomCss = response.Data.UseCustomCss,
                            Html = response.Data.Html,
                            UseHtml = response.Data.UseHtml,
                            CustomJavaScript = response.Data.CustomJavaScript,
                            UseCustomJavaScript = response.Data.UseCustomJavaScript
                        };

            return model;
        }
    }
}
