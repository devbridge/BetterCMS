// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleAnalyticsModuleConstants.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.GoogleAnalytics
{
    public static class GoogleAnalyticsModuleConstants
    {
        public const string LinkTypeKey = "LinkType";
        public const string DefaultLinkType = "alternate";

        public const string ChangeFrequencyKey = "ChangeFrequency";
        public const string DefaultChangeFrequency = "weekly";

        public const string PriorityKey = "Priority";
        public const string DefaultPriority = "0.5";

        public const string SitemapUrlKey = "SitemapUrl";
        public const string DefaultSitemapUrl = "sitemap.xml";

        public const string SitemapTitleKey = "SitemapTitle";
        public const string DefaultSitemapTitle = "Default Site Map";

        public const string SitemapDateFormatKey = "DateTimeFormat";
        public const string DefaultSitemapDateFormat = "yyyy-MM-dd";

        public const string GoogleAnalyticsScript = @"
  (function(i,s,o,g,r,a,m){{i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){{
  (i[r].q=i[r].q||[]).push(arguments)}},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  }})(window,document,'script','//www.google-analytics.com/analytics.js','ga');

  ga('create', '{0}', 'auto');
  ga('send', 'pageview');
";

        public const string KeyForAnalyticsKey = "AnalyticsKey";
    }
}