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