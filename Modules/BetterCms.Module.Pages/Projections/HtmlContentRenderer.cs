using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc.Helpers;

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
            if (Content.UseCustomCss && !string.IsNullOrWhiteSpace(Content.CustomCss))
            {
                var selectorPrefix = string.Concat("#", string.Format(RegionContentWrapper.PageContentIdPattern, Content.Id));
                var css = CssHelper.PrefixCssSelectors(Content.CustomCss, selectorPrefix);
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