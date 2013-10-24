using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc.Helpers;

namespace BetterCms.Module.Pages.Accessors
{
    [Serializable]
    public class PageJavaScriptAccessor : IJavaScriptAccessor
    {
        private readonly PageProperties page;

        private readonly IEnumerable<IOptionValue> options;

        public PageJavaScriptAccessor(PageProperties page, IEnumerable<IOptionValue> options)
        {
            this.page = page;
            this.options = options;
        }

        public string GetCustomJavaScript(HtmlHelper html)
        {
            if (page != null)
            {
                return page.CustomJS;
            }

            return null;
        }

        public string[] GetJavaScriptResources(HtmlHelper html)
        {
            if (options != null)
            {
                return options.ToJavaScriptResources();
            }

            return null;
        }
    }
}