using BetterCms.Module.Pages.Helpers.Extensions;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;

namespace BetterCms.Module.Pages.Mvc.PageHtmlRenderer
{
    public class RenderingPageCategoryProperty : RenderingPagePropertyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingPageCategoryProperty" /> class.
        /// </summary>
        public RenderingPageCategoryProperty()
            : base(RenderingPageProperties.PageCategory)
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
                        var category = model.GetPageCategoryModel();
                        return category != null ? category.Name : null;
                    });

            return stringBuilder;
        }
    }
}