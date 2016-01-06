namespace BetterCms.Module.Pages
{
    public static class PagesConstants
    {
        /// <summary>
        /// The intenal URL regular expression (used in page naming)
        /// </summary>
        // NOTE: After changing this regexp please run BetterCms.Test.Module.Pages.ServiceTests.UrlServiceTests test.
        public const string InternalUrlRegularExpression = @"^\/?([^\\:?#[\]@!$&'()*+,;=""<> \/%]{1,260}\/)*([^\\:?#[\]@!$&'()*+,;=""<> \/%]{1,260})?$";

        /// <summary>
        /// The external URL regular expression (used in sitemaps, external URLs)
        /// </summary>
        public const string ExternalUrlRegularExpression = @"^[^<>*&?]*((\?|#).*)?$";

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