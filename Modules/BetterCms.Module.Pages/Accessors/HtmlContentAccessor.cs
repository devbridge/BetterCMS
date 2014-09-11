using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Accessors
{
    [Serializable]
    public class HtmlContentAccessor : ContentAccessor<HtmlContent>
    {
        public const string ContentWrapperType = "html-content";

        public HtmlContentAccessor(HtmlContent content, IList<IOptionValue> options)
            : base(content, options)
        {
        }

        public override string GetContentWrapperType()
        {
            return ContentWrapperType;
        }

        public override string GetHtml(HtmlHelper html)
        {
            if (!string.IsNullOrWhiteSpace(Content.Html))
            {
                return Content.Html;
            }

            return "&nbsp;";
        }

        public override string[] GetCustomStyles(HtmlHelper html)
        {
            if (Content.UseCustomCss && !string.IsNullOrWhiteSpace(Content.CustomCss))
            {
                var css = CssHelper.FixCss(Content.CustomCss);
                if (!string.IsNullOrWhiteSpace(css))
                {
                    return new[] { css };
                }
            }

            return null;
        }

        public override string[] GetCustomJavaScript(HtmlHelper html)
        {
            if (Content.UseCustomJs && !string.IsNullOrWhiteSpace(Content.CustomJs))
            {
                return new[] { Content.CustomJs };
            }

            return null;
        }

        public override string[] GetStylesResources(HtmlHelper html)
        {
            return null;
        }

        public override string[] GetJavaScriptResources(HtmlHelper html)
        {
            return null;
        }
    }
}