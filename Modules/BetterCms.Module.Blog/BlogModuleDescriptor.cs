using System.Collections.Generic;
using System.Linq;

using Autofac;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Events;

using BetterCms.Module.Blog.Accessors;
using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Helpers.Extensions;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Models.Accessors;
using BetterCms.Module.Blog.Mvc.PageHtmlRenderer;
using BetterCms.Module.Blog.Registration;
using BetterCms.Module.Blog.Services;

using BetterCms.Module.Pages.Accessors;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Accessors;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;
using BetterCms.Module.Root.ViewModels.Cms;

using BetterModules.Core.Modules.Registration;

namespace BetterCms.Module.Blog
{
    /// <summary>
    /// Blog module descriptor
    /// </summary>
    public class BlogModuleDescriptor : CmsModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "blog";

        /// <summary>
        /// The blog area name
        /// </summary>
        internal const string BlogAreaName = "bcms-blog";
        
        /// <summary>
        /// The blog schema name
        /// </summary>
        internal const string BlogSchemaName = "bcms_blog";

        /// <summary>
        /// The blog java script module descriptor
        /// </summary>
        private readonly BlogJsModuleIncludeDescriptor blogJsModuleIncludeDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogModuleDescriptor" /> class.
        /// </summary>
        public BlogModuleDescriptor(ICmsConfiguration configuration) : base(configuration)
        {
            blogJsModuleIncludeDescriptor = new BlogJsModuleIncludeDescriptor(this);

            RootEvents.Instance.PageRetrieved += Events_PageRetrieved;

            CategoryAccessors.Register<BlogCategoryAccessor>();
            RegisterRenderingPageProperties();
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
        /// Gets the name of the module area.
        /// </summary>
        /// <value>
        /// The name of the module area.
        /// </value>
        public override string AreaName
        {
            get
            {
                return BlogAreaName;
            }
        }

        /// <summary>
        /// Gets the name of the module database schema name.
        /// </summary>
        /// <value>
        /// The name of the module database schema.
        /// </value>
        public override string SchemaName
        {
            get
            {
                return BlogSchemaName;
            }
        }

        /// <summary>
        /// Registers the sidebar main projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns></returns>
        public override IEnumerable<IPageActionProjection> RegisterSidebarMainProjections(ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
                {
                    new ButtonActionProjection(blogJsModuleIncludeDescriptor, page => "postNewArticle")
                        {
                            Title = page => BlogGlobalization.Sidebar_AddNewPostButtonTitle,
                            Order = 200,
                            CssClass = page => "bcms-sidemenu-btn bcms-btn-blog-add",
                            AccessRole = RootModuleConstants.UserRoles.EditContent
                        }
                };
        }

        /// <summary>
        /// Registers java script modules.
        /// </summary>        
        /// <returns>
        /// Enumerator of known JS modules list.
        /// </returns>
        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new[]
                {
                    blogJsModuleIncludeDescriptor,
                    new JsIncludeDescriptor(this, "bcms.blog.filter"), 
                };
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>        
        /// <returns>Enumerator of known module style sheet files.</returns>
        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[]
                {
                    new CssIncludeDescriptor(this, "bcms.blog.css")
                };
        }

        /// <summary>
        /// Registers the site settings projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns>List of page action projections.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
                {
                    new LinkActionProjection(blogJsModuleIncludeDescriptor, page => "loadSiteSettingsBlogs")
                        {
                            Order = 1200,
                            Title = page => BlogGlobalization.SiteSettings_BlogsMenuItem,
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
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            RegisterContentRendererType<BlogPostContentAccessor, BlogPostContent>(containerBuilder);
            RegisterStylesheetRendererType<PageStylesheetAccessor, BlogPost>(containerBuilder);
            RegisterJavaScriptRendererType<PageJavaScriptAccessor, BlogPost>(containerBuilder);

            containerBuilder.RegisterType<DefaultOptionService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultAuthorService>().AsImplementedInterfaces().InstancePerLifetimeScope(); 
            containerBuilder.RegisterType<DefaultBlogService>().As<IBlogService>().InstancePerLifetimeScope();          
            containerBuilder.RegisterType<DefaultBlogSaveService>().As<IBlogSaveService>().InstancePerLifetimeScope();          
            containerBuilder.RegisterType<DefaultBlogMLService>().AsImplementedInterfaces().InstancePerLifetimeScope();          
            containerBuilder.RegisterType<DefaultBlogMLExportService>().AsImplementedInterfaces().InstancePerLifetimeScope();          
        }

        /// <summary>
        /// Occurs, when the page is retrieved.
        /// </summary>
        /// <param name="args">The <see cref="PageRetrievedEventArgs" /> instance containing the event data.</param>
        private void Events_PageRetrieved(PageRetrievedEventArgs args)
        {
            if (args == null || args.RenderPageData == null)
            {
                return;
            }

            ExtendPageWithPageData(args.RenderPageData, args.PageData);

            if (!args.RenderPageData.IsBlogPost())
            {
                return; // Default handling.
            }

            if (args.RenderPageData.CanManageContent)
            {
                return; // Default handling.
            }

            if (args.RenderPageData.Status != PageStatus.Published)
            {
                return; // Default handling.
            }

            if (args.RenderPageData.Contents.Any(projection => projection.PageContentStatus != ContentStatus.Published))
            {
                return; // Default handling.
            }

            if (!args.RenderPageData.IsBlogPostActive())
            {
                args.EventResult = PageRetrievedEventResult.ForcePageNotFound;
            }
        }

        /// <summary>
        /// Extends the page and master page view models with data from provided page entity.
        /// </summary>
        /// <param name="renderPageViewModel">The render page view model.</param>
        /// <param name="pageData">The page data.</param>
        private void ExtendPageWithPageData(RenderPageViewModel renderPageViewModel, IPage pageData)
        {
            if (renderPageViewModel.MasterPage != null)
            {
                ExtendPageWithPageData(renderPageViewModel.MasterPage, renderPageViewModel.PageData);
            }

            renderPageViewModel.ExtendWithBlogData(pageData);
        }

        private void RegisterRenderingPageProperties()
        {
            PageHtmlRenderer.Register(new RenderingPageAuthorProperty());
            PageHtmlRenderer.Register(new RenderingPageBlogExpirationDateProperty());
            PageHtmlRenderer.Register(new RenderingPageBlogActivationDateProperty());
        }
    }
}
