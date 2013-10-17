namespace BetterCms.Module.Pages
{
    public static class PagesConstants
    {
        /// <summary>
        /// The page URL regular expression
        /// </summary>
        public const string PageUrlRegularExpression = @"^([^:?#[\]@!$&'()*+,;= /%]{0,260}/)*[^:?#[\]@!$&'()*+,;= /%]{0,260}$";

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