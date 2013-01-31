using System;
using System.Web.Mvc;

using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Accessors
{
    [Serializable]
    public class PageStylesheetAccessor : IStylesheetAccessor
    {
        private readonly PageProperties page;

        public PageStylesheetAccessor(PageProperties page)
        {
            this.page = page;
        }

        public string GetCustomStyles(HtmlHelper html)
        {
            if (page != null)
            {
                return page.CustomCss;
            }

            return null;
        }
    }
}