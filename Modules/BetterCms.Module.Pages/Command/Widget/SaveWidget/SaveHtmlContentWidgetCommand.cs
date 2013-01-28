using System.Linq;

using BetterCms.Core.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Pages.Command.Widget.SaveWidget
{
    public class SaveHtmlContentWidgetCommand : SaveWidgetCommandBase<HtmlContentWidgetViewModel>
    {
        public virtual IContentService ContentService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override SaveWidgetResponse Execute(HtmlContentWidgetViewModel request)
        {
            UnitOfWork.BeginTransaction();
            HtmlContentWidget content = (HtmlContentWidget)ContentService.SaveContentWithStatusUpdate(GetHtmlContentWidgetFromRequest(request), request.DesirableStatus);
            Repository.Save(content);
            UnitOfWork.Commit();

            return new SaveWidgetResponse
                    {
                        Id = content.Id,
                        WidgetName = content.Name,
                        CategoryName = content.Category != null ? content.Category.Name : null,
                        Version = content.Version,
                        WidgetType = WidgetType.HtmlContent.ToString(),
                        IsPublished = content.Status == ContentStatus.Published,
                        HasDraft = content.Status == ContentStatus.Draft || content.History != null && content.History.Any(f => f.Status == ContentStatus.Draft)
                    };
        }

        private HtmlContentWidget GetHtmlContentWidgetFromRequest(HtmlContentWidgetViewModel request)
        {
            HtmlContentWidget content = new HtmlContentWidget();
            content.Id = request.Id;

            if (request.CategoryId.HasValue && !request.CategoryId.Value.HasDefaultValue())
            {
                content.Category = Repository.AsProxy<Category>(request.CategoryId.Value);
            }
            else
            {
                content.Category = null;
            }

            content.Name = request.Name;
            content.Html = request.PageContent ?? string.Empty;
            content.UseHtml = request.EnableCustomHtml;
            content.UseCustomCss = request.EnableCustomCSS;
            content.CustomCss = request.CustomCSS;            
            content.UseCustomJs = request.EnableCustomJS;
            content.CustomJs = request.CustomJS;           
            content.Version = request.Version;

            return content;
        }
    }
}