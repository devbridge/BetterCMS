using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Accessors
{
    [Serializable]
    public class HtmlContentWidgetAccessor : ContentAccessor<HtmlContentWidget>
    {
        public HtmlContentWidgetAccessor(HtmlContentWidget content, IList<IOption> options)
            : base(content, options)
        {
        }

        public override string GetRegionWrapperCssClass(HtmlHelper html)
        {
            return "bcms-content-advanced";
        }

        public override string GetHtml(HtmlHelper html)
        {

            if (Content.UseHtml && !string.IsNullOrWhiteSpace(Content.Html))
            {
                return Content.Html;
            }

            return "&nbsp;";
        }

        public override string GetCustomStyles(HtmlHelper html)
        {
            if (Content.UseCustomCss && !string.IsNullOrWhiteSpace(Content.CustomCss))
            {
                var css = CssHelper.FixCss(Content.CustomCss);
                if (!string.IsNullOrWhiteSpace(css))
                {
                    return css;
                }
            }

            return null;
        }

        public override string GetCustomJavaScript(HtmlHelper html)
        {
            if (Content.UseCustomJs && !string.IsNullOrWhiteSpace(Content.CustomJs))
            {
                return Content.CustomJs;
            }

            return null;
        }
    }
}