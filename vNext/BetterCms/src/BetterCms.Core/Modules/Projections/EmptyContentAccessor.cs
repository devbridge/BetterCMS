using BetterCms.Core.DataContracts;
using Microsoft.AspNet.Mvc.ViewFeatures;

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

        public override string GetContentWrapperType()
        {
            return null;
        }
        
        public override string GetTitle()
        {
            return null;
        }

        public override string GetHtml(HtmlHelper html)
        {
            return string.Format(contentHtml);
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
    }
}
