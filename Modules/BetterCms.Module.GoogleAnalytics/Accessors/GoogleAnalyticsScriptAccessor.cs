using System.Web.Mvc;

using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.GoogleAnalytics.Accessors
{
    public class GoogleAnalyticsScriptAccessor : IJavaScriptAccessor
    {
        private ICmsConfiguration cmsConfiguration;

        public GoogleAnalyticsScriptAccessor(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
        }

        public string[] GetCustomJavaScript(HtmlHelper html)
        {
            return new[] { string.Format(GoogleAnalyticsModuleConstants.GoogleAnalyticsScript, GoogleAnalyticsModuleHelper.GetAnalyticsKey(cmsConfiguration)) };
        }

        public string[] GetJavaScriptResources(HtmlHelper html)
        {
            return null;
        }
    }
}