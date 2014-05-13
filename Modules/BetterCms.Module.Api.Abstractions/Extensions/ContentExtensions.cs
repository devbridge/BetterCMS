using BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent;

namespace BetterCms.Module.Api.Extensions
{
    public static class ContentExtensions
    {
        public static PutHtmlContentRequest ToPutRequest(this GetHtmlContentResponse response)
        {
            var model = MapHtmlContentWidgetModel(response);

            return new PutHtmlContentRequest { Data = model, ContentId = response.Data.Id };
        }
        private static SaveHtmlContentModel MapHtmlContentWidgetModel(GetHtmlContentResponse response)
        {
            var model = new SaveHtmlContentModel
                        {
                            Version = response.Data.Version,
                            Name = response.Data.Name,
                            ActivationDate = response.Data.ActivationDate,
                            ExpirationDate = response.Data.ExpirationDate,
                            IsPublished = response.Data.IsPublished,
                            PublishedOn = response.Data.PublishedOn,
                            PublishedByUser = response.Data.PublishedByUser,
                            Html = response.Data.Html,
                            CustomCss = response.Data.CustomCss,
                            UseCustomCss = response.Data.UseCustomCss,
                            CustomJavaScript = response.Data.CustomJavaScript,
                            UseCustomJavaScript = response.Data.UseCustomJavaScript,
                        };

            return model;
        }
    }
}
