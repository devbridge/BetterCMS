using System.Collections.Generic;

using Autofac;

using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Blog.Accessors;
using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Registration;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Pages.Accessors;
using BetterCms.Module.Root;

namespace BetterCms.Module.Blog
{
    /// <summary>
    /// Blog module descriptor
    /// </summary>
    public class BlogModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "blog";

        /// <summary>
        /// The blog java script module descriptor
        /// </summary>
        private readonly BlogJavaScriptModuleDescriptor blogJavaScriptModuleDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogModuleDescriptor" /> class.
        /// </summary>
        public BlogModuleDescriptor()
        {
            blogJavaScriptModuleDescriptor = new BlogJavaScriptModuleDescriptor(this);
        }

        /// <summary>
        /// Gets the module name.
        /// </summary>
        /// <value>
        /// The module name.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }

        /// <summary>
        /// Gets the module description.
        /// </summary>
        /// <value>
        /// The module description.
        /// </value>
        public override string Description
        {
            get
            {
                return "Blog module for BetterCMS.";
            }
        }

        /// <summary>
        /// Registers the sidebar main projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public override IEnumerable<IPageActionProjection> RegisterSidebarMainProjections(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new IPageActionProjection[]
                {
                    new ButtonActionProjection(blogJavaScriptModuleDescriptor, page => "postNewArticle")
                        {
                            Title = () => BlogGlobalization.Sidebar_AddNewPostButtonTitle,
                            Order = 200,
                            CssClass = page => "bcms-sidemenu-btn bcms-btn-blog-add",
                            AccessRole = RootModuleConstants.UserRoles.EditContent
                        }
                };
        }

        /// <summary>
        /// Registers java script modules.
        /// </summary>
        /// <param name="configuration">The CMS configuration.</param>
        /// <returns>
        /// Enumerator of known JS modules list.
        /// </returns>
        public override IEnumerable<JavaScriptModuleDescriptor> RegisterJavaScriptModules(ICmsConfiguration configuration)
        {
            return new[]
                {
                    blogJavaScriptModuleDescriptor
                };
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Enumerator of known module style sheet files.</returns>
        public override IEnumerable<string> RegisterStyleSheetFiles(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new[]
                {
                    "/file/bcms-blog/Content/Styles/bcms.blog.css"
                };
        }

        /// <summary>
        /// Registers the site settings projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>List of page action projections.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new IPageActionProjection[]
                {
                    new LinkActionProjection(blogJavaScriptModuleDescriptor, page => "loadSiteSettingsBlogs")
                        {
                            Order = 1200,
                            Title = () => BlogGlobalization.SiteSettings_BlogsMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.DeleteContent)
                        }                                      
                };
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            RegisterContentRendererType<BlogPostContentAccessor, BlogPostContent>(containerBuilder);
            RegisterStylesheetRendererType<PageStylesheetAccessor, BlogPost>(containerBuilder);
            RegisterJavaScriptRendererType<PageJavaScriptAccessor, BlogPost>(containerBuilder);

            containerBuilder.RegisterType<DefaultOptionService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultAuthorService>().AsImplementedInterfaces().InstancePerLifetimeScope(); 
            containerBuilder.RegisterType<DefaultBlogService>().AsImplementedInterfaces().InstancePerLifetimeScope();          
        }
    }
}
