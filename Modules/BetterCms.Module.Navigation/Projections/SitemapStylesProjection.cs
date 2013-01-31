using System.Web.Mvc;

using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Projections
{
    public class PageStylesProjection : IStylesheetAccessor
    {
        private readonly PageProperties page;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageStylesProjection" /> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public PageStylesProjection(PageProperties page)
        {
            this.page = page;
        }

        public string GetCustomStyles(HtmlHelper html)
        {
            if (page != null && page.UseCustomCss)
            {
                return page.CustomCss;
            }
            return null;
        }      
    }
}