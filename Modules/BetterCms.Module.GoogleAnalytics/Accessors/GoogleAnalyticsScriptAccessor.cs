using System.Web.Mvc;

using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.GoogleAnalytics.Accessors
{
    public class GoogleAnalyticsScriptAccessor : IJavaScriptAccessor
    {
        private ICmsConfiguration _cmsConfiguration;

        public GoogleAnalyticsScriptAccessor(ICmsConfiguration cmsConfiguration)
        {
            _cmsConfiguration = cmsConfiguration;
        }

        public string GetCustomJavaScript(HtmlHelper html)
        {
            return string.Format(GoogleAnalyticsModuleConstants.GoogleAnalyticsScript, GoogleAnalyticsModuleHelper.GetAnalyticsKey(_cmsConfiguration));
        }

        public string[] GetJavaScriptResources(HtmlHelper html)
        {
            return null;
        }
    }
}