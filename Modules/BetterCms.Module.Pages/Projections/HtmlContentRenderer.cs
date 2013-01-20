using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Projections
{
    [Serializable]
    public class HtmlContentAccessor : ContentAccessor<HtmlContent>
    {
        public HtmlContentAccessor(HtmlContent content, IList<IPageContentOption> options)
            : base(content, options)
        {
        }

        public override string GetRegionWrapperCssClass(HtmlHelper html)
        {
            return "bcms-content-regular";
        }

        public override string GetHtml(HtmlHelper html)
        {
            if (!string.IsNullOrWhiteSpace(Content.Html))
            {
                return Content.Html;
            }

            return "&nbsp;";
        }

        public override string GetCustomStyles(HtmlHelper html)
        {
            return null;
        }

        public override string GetCustomJavaScript(HtmlHelper html)
        {
            return null;
        }
    }
}