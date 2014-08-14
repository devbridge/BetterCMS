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
    public class PageStylesheetAccessor : IStylesheetAccessor
    {
        private readonly PageProperties page;

        private readonly IEnumerable<IOptionValue> options;

        public PageStylesheetAccessor(PageProperties page, IEnumerable<IOptionValue> options)
        {
            this.page = page;
            this.options = options;
        }

        public string[] GetCustomStyles(HtmlHelper html)
        {
            if (page != null && !string.IsNullOrWhiteSpace(page.CustomCss))
            {
                return new[] { page.CustomCss };
            }

            return null;
        }

        public string[] GetStylesResources(HtmlHelper html)
        {
            if (options != null)
            {
                return options.ToStyleResources();
            }

            return null;
        }
    }
}