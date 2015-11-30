// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleAnalyticsScriptAccessor.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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