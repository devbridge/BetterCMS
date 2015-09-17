using BetterCms.Module.Pages.Helpers.Extensions;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;

namespace BetterCms.Module.Pages.Mvc.PageHtmlRenderer
{
    public class RenderingPageSecondaryImageUrlProperty : RenderingPagePropertyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingPageSecondaryImageUrlProperty" /> class.
        /// </summary>
        public RenderingPageSecondaryImageUrlProperty()
            : base(RenderingPageProperties.SecondaryImageUrl)
        {
        }

        /// <summary>
        /// Gets the replaced HTML.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="model">The model.</param>
        /// <returns>HTML with replaced model values</returns>
        public override System.Text.StringBuilder GetReplacedHtml(System.Text.StringBuilder stringBuilder, Root.ViewModels.Cms.RenderPageViewModel model)
        {
            stringBuilder = GetReplacedHtml(stringBuilder,
                () =>
                    {
                        var image = model.GetPageSecondaryImageModel();
                        return image != null ? image.PublicUrl : null;
                    });

            return stringBuilder;
        }
    }
}