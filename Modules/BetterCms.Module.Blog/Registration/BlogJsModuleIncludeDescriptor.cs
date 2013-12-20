using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Controllers;

namespace BetterCms.Module.Blog.Registration
{
    /// <summary>
    /// bcms.blog.js module descriptor.
    /// </summary>
    public class BlogJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public BlogJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.blog")
        {
            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<BlogController>(this, "loadSiteSettingsBlogsUrl", c => c.Index(null)),
                            new JavaScriptModuleLinkTo<BlogController>(this, "loadCreateNewPostDialogUrl", c => c.CreateBlogPost("{0}")),
                            new JavaScriptModuleLinkTo<BlogController>(this, "loadEditPostDialogUrl", c => c.EditBlogPost("{0}")),
                            new JavaScriptModuleLinkTo<AuthorController>(this, "loadAuthorsTemplateUrl", c => c.ListTemplate()),
                            new JavaScriptModuleLinkTo<AuthorController>(this, "loadAuthorsUrl", c => c.AuthorsList(null)),
                            new JavaScriptModuleLinkTo<AuthorController>(this, "deleteAuthorsUrl", c => c.DeleteAuthor(null, null)),
                            new JavaScriptModuleLinkTo<AuthorController>(this, "saveAuthorsUrl", c => c.SaveAuthor(null)),
                            new JavaScriptModuleLinkTo<OptionController>(this, "loadTemplatesUrl", c => c.Templates()),
                            new JavaScriptModuleLinkTo<OptionController>(this, "saveDefaultTemplateUrl", c => c.SaveDefaultTemplate("{0}", "{1}")),
                            new JavaScriptModuleLinkTo<BlogController>(this, "convertStringToSlugUrl", c => c.ConvertStringToSlug("{0}", "{1}"))
                        };

            Globalization = new IActionProjection[]
                        {
                            new JavaScriptModuleGlobalization(this, "createNewPostDialogTitle", () => BlogGlobalization.CreateNewPost_Dialog_Title),
                            new JavaScriptModuleGlobalization(this, "editPostDialogTitle", () => BlogGlobalization.EditPost_Dialog_Title),
                            new JavaScriptModuleGlobalization(this, "deleteBlogDialogTitle", () => BlogGlobalization.DeletePost_Dialog_Title),
                            new JavaScriptModuleGlobalization(this, "deleteAuthorDialogTitle", () => BlogGlobalization.DeleteAuthor_Confirmation_Message),
                            new JavaScriptModuleGlobalization(this, "blogPostsTabTitle", () => BlogGlobalization.SiteSettings_Blogs_PostsTab_Title),
                            new JavaScriptModuleGlobalization(this, "authorsTabTitle", () => BlogGlobalization.SiteSettings_Blogs_AuthorsTab_Title),
                            new JavaScriptModuleGlobalization(this, "templatesTabTitle", () => BlogGlobalization.SiteSettings_Blogs_TemplatesTab_Title),
                            new JavaScriptModuleGlobalization(this, "datePickerTooltipTitle", () => BlogGlobalization.Date_Picker_Tooltip_Title)
                        };
        }
    }
}