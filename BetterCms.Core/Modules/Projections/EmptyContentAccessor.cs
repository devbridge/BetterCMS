using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Api.Interfaces.Models;
using BetterCms.Core.Models;

namespace BetterCms.Core.Modules.Projections
{
    public class EmptyContentAccessor : ContentAccessor<IContent>
    {
        private readonly string contentHtml;

        public EmptyContentAccessor(string contentHtml)
            : base(null, null)
        {            
            this.contentHtml = contentHtml;
        }

        public override string GetRegionWrapperCssClass(HtmlHelper html)
        {
            return null;
        }

        public override string GetHtml(HtmlHelper html)
        {
            return string.Format(contentHtml);
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
