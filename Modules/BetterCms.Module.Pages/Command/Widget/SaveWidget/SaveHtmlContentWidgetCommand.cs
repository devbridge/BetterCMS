using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Widget.SaveWidget
{
    public class SaveHtmlContentWidgetCommand : SaveWidgetCommandBase<HtmlContentWidgetViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override SaveWidgetResponse Execute(HtmlContentWidgetViewModel request)
        {
            // 1.
            var content = !request.Id.HasDefaultValue()
                ? Repository.First<HtmlContentWidget>(request.Id) 
                : new HtmlContentWidget();

            // 2.
            UpdateWidget(content, request);

            // 3.
            UnitOfWork.BeginTransaction();
            Repository.Save(content);
            UnitOfWork.Commit();

            return new SaveWidgetResponse
                {
                    Id = content.Id,
                    WidgetName = content.Name,
                    CategoryName = content.Category != null ? content.Category.Name : null,
                    Version = content.Version,
                    WidgetType = WidgetType.HtmlContent.ToString()
                };
        }

        private HtmlContentWidget UpdateWidget(HtmlContentWidget content, HtmlContentWidgetViewModel request)
        {
            if (request.CategoryId.HasValue && !request.CategoryId.Value.HasDefaultValue())
            {
                content.Category = Repository.FirstOrDefault<Category>(request.CategoryId.Value);
            }
            else
            {
                content.Category = null;
            }

            content.Name = request.Name;
            content.Html = request.PageContent ?? string.Empty;
            content.UseHtml = request.EnableCustomHtml;
            content.CustomCss = request.CustomCSS;
            content.UseCustomCss = request.EnableCustomCSS;
            content.UseCustomJs = request.EnableCustomJS;
            content.CustomJs = request.CustomJS;
            content.Version = request.Version;

            return content;
        }
    }
}