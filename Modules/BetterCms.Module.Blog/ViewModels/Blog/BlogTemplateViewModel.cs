using BetterCms.Module.Pages.ViewModels.Page;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class BlogTemplateViewModel : TemplateViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether template is compatible for blog post.
        /// </summary>
        /// <value>
        /// <c>true</c> if template is incompatible for blog post; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsCompatible { get; set; }
    }
}