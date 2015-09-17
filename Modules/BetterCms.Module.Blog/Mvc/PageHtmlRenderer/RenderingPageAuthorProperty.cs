using BetterCms.Module.Blog.Helpers.Extensions;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;

namespace BetterCms.Module.Blog.Mvc.PageHtmlRenderer
{
    public class RenderingPageAuthorProperty : RenderingPagePropertyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingPageAuthorProperty" /> class.
        /// </summary>
        public RenderingPageAuthorProperty()
            : base(RenderingPageProperties.BlogAuthor)
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
                        var author = model.GetBlogPostAuthorModel();
                        return author != null ? author.Name : null;
                    });

            return stringBuilder;
        }
    }
}