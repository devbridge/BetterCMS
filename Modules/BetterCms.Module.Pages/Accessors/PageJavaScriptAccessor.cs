using System;
using System.Web.Mvc;

using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Accessors
{
    [Serializable]
    public class PageJavaScriptAccessor : IJavaScriptAccessor
    {
        private readonly PageProperties page;

        public PageJavaScriptAccessor(PageProperties page)
        {
            this.page = page;
        }

        public string GetCustomJavaScript(HtmlHelper html)
        {
            if (page != null)
            {
                return page.CustomJS;
            }

            return null;
        }
    }
}