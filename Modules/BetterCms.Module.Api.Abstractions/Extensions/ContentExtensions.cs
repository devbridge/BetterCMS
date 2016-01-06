using BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content;

namespace BetterCms.Module.Api.Extensions
{
    public static class ContentExtensions
    {
        public static PutHtmlContentRequest ToPutRequest(this GetHtmlContentResponse response)
        {
            var model = MapHtmlContentWidgetModel(response);

            return new PutHtmlContentRequest { Data = model, Id = response.Data.Id };
        }

        public static PutPageContentRequest ToPutRequest(this GetPageContentResponse response)
        {
            var model = MapPageContentWidgetModel(response);

            return new PutPageContentRequest { Data = model, PageId = response.Data.PageId, Id = response.Data.Id };
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
                            OriginalText = response.Data.OriginalText,
                            ContentTextMode = response.Data.ContentTextMode,
                            CustomCss = response.Data.CustomCss,
                            UseCustomCss = response.Data.UseCustomCss,
                            CustomJavaScript = response.Data.CustomJavaScript,
                            UseCustomJavaScript = response.Data.UseCustomJavaScript,
                            ChildContentsOptionValues = response.ChildContentsOptionValues
                        };

            return model;
        }

        private static SavePageContentModel MapPageContentWidgetModel(GetPageContentResponse response)
        {
            var model = new SavePageContentModel
                {
                    Version = response.Data.Version,
                    ContentId = response.Data.ContentId,
                    ParentPageContentId = response.Data.ParentPageContentId,
                    RegionId = response.Data.RegionId,
                    Order = response.Data.Order,
                    Options = response.Options
                };

            return model;
        }
    }
}
