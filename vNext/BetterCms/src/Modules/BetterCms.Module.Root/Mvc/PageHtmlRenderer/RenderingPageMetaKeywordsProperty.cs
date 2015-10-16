namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    public class RenderingPageMetaKeywordsProperty : RenderingPagePropertyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingPageMetaKeywordsProperty" /> class.
        /// </summary>
        public RenderingPageMetaKeywordsProperty()
            : base(RenderingPageProperties.MetaKeywords)
        {
        }

        /// <summary>
        /// Gets the replaced HTML.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="model">The model.</param>
        /// <returns>HTML with replaced model values</returns>
        public override System.Text.StringBuilder GetReplacedHtml(System.Text.StringBuilder stringBuilder, ViewModels.Cms.RenderPageViewModel model)
        {
            return GetReplacedHtml(stringBuilder, () => model.MetaKeywords);
        }
    }
}