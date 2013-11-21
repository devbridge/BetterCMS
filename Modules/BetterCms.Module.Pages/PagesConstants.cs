namespace BetterCms.Module.Pages
{
    public static class PagesConstants
    {
        /// <summary>
        /// The page URL regular expression
        /// </summary>
        // NOTE: After changing this regexp please run BetterCms.Test.Module.Pages.ServiceTests.UrlServiceTests test.
        public const string PageUrlRegularExpression = @"^\/?([^\\:?#[\]@!$&'()*+,;= \/%]{1,260}\/)*([^\\:?#[\]@!$&'()*+,;= \/%]{1,260})?$";

        /// <summary>
        /// The site map URL regular expression
        /// </summary>
        public const string SiteMapUrlRegularExpression = @"^[^<>*&?]*((\?|#).*)?$";

        /// <summary>
        /// The options grid template.
        /// </summary>
        public const string OptionsGridTemplate = "~/Areas/bcms-pages/Views/Option/EditOptions.cshtml";
        
        /// <summary>
        /// The option values grid template.
        /// </summary>
        public const string OptionValuesGridTemplate = "~/Areas/bcms-pages/Views/Option/EditOptionValues.cshtml";

        /// <summary>
        /// The content version preview template
        /// </summary>
        public const string ContentVersionPreviewTemplate = "~/Areas/bcms-pages/Views/History/ContentVersion.cshtml";
    }
}