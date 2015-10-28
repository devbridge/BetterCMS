using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Pages.ViewModels.History;

namespace BetterCms.Module.Blog.Accessors
{
    [Serializable]
    public class BlogPostContentAccessor : ContentAccessor<BlogPostContent>
    {
        public const string ContentWrapperType = "blog-post-content";

        public BlogPostContentAccessor(BlogPostContent content, IList<IOptionValue> options)
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
            return null;
        }

        public override string[] GetCustomJavaScript(HtmlHelper html)
        {
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

        public override PropertiesPreview GetHtmlPropertiesPreview()
        {
            return new PropertiesPreview
            {
                ViewName = "~/Areas/bcms-blog/Views/History/BlogPropertiesHistory.cshtml",
                ViewModel = new HtmlContentHistoryViewModel
                {
                    Name = Content.Name,
                    ActivationDate = Content.ActivationDate,
                    ExpirationDate = Content.ExpirationDate,
                    Html = Content.Html,
                    UseCustomCss = Content.UseCustomCss,
                    CustomCss = Content.CustomCss,
                    UseCustomJs = Content.UseCustomJs,
                    CustomJs = Content.CustomJs,
                }
            };
        }
    }
}