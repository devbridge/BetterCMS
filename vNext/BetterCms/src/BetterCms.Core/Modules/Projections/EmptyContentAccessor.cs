using BetterCms.Core.DataContracts;
using Microsoft.AspNet.Mvc.Rendering;
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

        public override string GetHtml(IHtmlHelper html)
        {
            return string.Format(contentHtml);
        }

        public override string[] GetCustomStyles(IHtmlHelper html)
        {
            return null;
        }

        public override string[] GetCustomJavaScript(IHtmlHelper html)
        {
            return null;
        }

        public override string[] GetStylesResources(IHtmlHelper html)
        {
            return null;
        }

        public override string[] GetJavaScriptResources(IHtmlHelper html)
        {
            return null;
        }
    }
}
