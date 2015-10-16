using System.Web;
using System.Web.Mvc;

using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Root.Mvc.Grids.Extensions
{
    public static class PagingExtensions
    {
        /// <summary>
        /// Adds the paging to page.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TGridItem">The type of the grid item.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns>Generated HTML string with paging</returns>
        public static IHtmlString RenderPaging<TModel, TGridItem>(this HtmlHelper<TModel> htmlHelper, SearchableGridViewModel<TGridItem> model)
            where TGridItem : IEditableGridItem
        {
            var pages = PagerHelper.RenderPager(model);
            return new MvcHtmlString(pages);
        }
    }
}