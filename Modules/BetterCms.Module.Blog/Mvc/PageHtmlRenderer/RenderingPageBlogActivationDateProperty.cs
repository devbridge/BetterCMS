using BetterCms.Module.Blog.Helpers.Extensions;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;

namespace BetterCms.Module.Blog.Mvc.PageHtmlRenderer
{
    public class RenderingPageBlogActivationDateProperty : RenderingPagePropertyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingPageBlogActivationDateProperty" /> class.
        /// </summary>
        public RenderingPageBlogActivationDateProperty()
            : base(RenderingPageProperties.BlogActivationDate)
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
            var blog = model.GetBlogPostModel();
            stringBuilder = GetReplacedHtml(stringBuilder, blog != null ? blog.ActivationDate : (System.DateTime?)null);

            return stringBuilder;
        }
    }
}