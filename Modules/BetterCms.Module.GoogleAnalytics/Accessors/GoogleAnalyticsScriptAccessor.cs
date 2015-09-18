using System;
using System.Web.Mvc;

using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.GoogleAnalytics.Accessors
{
    public class GoogleAnalyticsScriptAccessor : IJavaScriptAccessor
    {
        private readonly Guid analyticsScriptGuid;
        private ICmsConfiguration cmsConfiguration;

        public GoogleAnalyticsScriptAccessor(ICmsConfiguration cmsConfiguration, Guid analyticsScriptGuid)
        {
            this.cmsConfiguration = cmsConfiguration;
            this.analyticsScriptGuid = analyticsScriptGuid;
        }

        public string[] GetCustomJavaScript(HtmlHelper html)
        {
            var analyticsKey = GoogleAnalyticsModuleHelper.GetAnalyticsKey(cmsConfiguration);

            if (!string.IsNullOrWhiteSpace(analyticsKey))
            {
                return new[] { string.Format(GoogleAnalyticsModuleConstants.GoogleAnalyticsScript, analyticsKey) };
            }

            return null;
        }

        public string[] GetJavaScriptResources(HtmlHelper html)
        {
            return null;
        }

        protected bool Equals(GoogleAnalyticsScriptAccessor other)
        {
            return analyticsScriptGuid.Equals(other.analyticsScriptGuid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((GoogleAnalyticsScriptAccessor)obj);
        }

        public override int GetHashCode()
        {
            return analyticsScriptGuid.GetHashCode();
        }
    }
}