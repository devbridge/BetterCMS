namespace BetterCms.Module.Pages
{
    public static class PagesConstants
    {
        /// <summary>
        /// The page URL regular expression
        /// </summary>
        public const string PageUrlRegularExpression = @"(^/$)|((?!.*//)(^((/{1}|)[\w\-]{0,260})+(/{1}$|)))";

        /// <summary>
        /// The site map URL regular expression
        /// </summary>
        public const string SiteMapUrlRegularExpression = @"^/?[\w\-/].*/?";

        /// <summary>
        /// The options grid template.
        /// </summary>
        public const string OptionsGridTemplate = "~/Areas/bcms-pages/Views/Option/EditOptions.cshtml";
        
        /// <summary>
        /// The option values grid template.
        /// </summary>
        public const string OptionValuesGridTemplate = "~/Areas/bcms-pages/Views/Option/EditOptionValues.cshtml";
    }
}