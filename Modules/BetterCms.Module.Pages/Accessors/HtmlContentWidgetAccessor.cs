using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;

namespace BetterCms.Module.Pages.Accessors
{
    [Serializable]
    public class HtmlContentWidgetAccessor : ContentAccessor<HtmlContentWidget>
    {
        public const string ContentWrapperType = "html-widget";

        public HtmlContentWidgetAccessor(HtmlContentWidget content, IList<IOptionValue> options)
            : base(content, options)
        {
        }

        public override string GetContentWrapperType()
        {
            return ContentWrapperType;
        }

        public override string GetHtml(HtmlHelper html)
        {
            if (Content.UseHtml && !string.IsNullOrWhiteSpace(Content.Html))
            {
                var builder = new StringBuilder(Content.Html);
                builder = PageContentHtmlRenderer.GetReplacedHtml(builder, Options);

                return builder.ToString();
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
            if (Options != null)
            {
                return Options.ToStyleResources();
            }

            return null;
        }

        public override string[] GetJavaScriptResources(HtmlHelper html)
        {
            if (Options != null)
            {
                return Options.ToJavaScriptResources();
            }

            return null;
        }
    }
}