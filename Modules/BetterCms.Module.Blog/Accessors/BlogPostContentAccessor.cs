using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.Accessors
{
    [Serializable]
    public class BlogPostContentAccessor : ContentAccessor<BlogPostContent>
    {
        public BlogPostContentAccessor(BlogPostContent content, IList<IOption> options)
            : base(content, options)
        {
        }

        public override string GetContentWrapperType()
        {
            return "blog-post-content";
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