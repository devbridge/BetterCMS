using System;
using System.Reflection;

using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;

using BetterCms.Core.Modules;

using BetterCms.Module.Api.Operations.Blog;
using BetterCms.Module.Api.Operations.Blog.Authors;
using BetterCms.Module.Api.Operations.Blog.Authors.Author;
using BetterCms.Module.Api.Operations.Blog.BlogPosts;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Content;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings;
using BetterCms.Module.Api.Operations.MediaManager;
using BetterCms.Module.Api.Operations.MediaManager.Files;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.MediaManager.Folders;
using BetterCms.Module.Api.Operations.MediaManager.Folders.Folder;
using BetterCms.Module.Api.Operations.MediaManager.Images;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.Api.Operations.MediaManager.MediaTree;
using BetterCms.Module.Api.Operations.Pages;
using BetterCms.Module.Api.Operations.Pages.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.Draft;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.History;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent;
using BetterCms.Module.Api.Operations.Pages.Pages;
using BetterCms.Module.Api.Operations.Pages.Pages.Page;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content.Options;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations;
using BetterCms.Module.Api.Operations.Pages.Pages.Search;
using BetterCms.Module.Api.Operations.Pages.Redirects;
using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;
using BetterCms.Module.Api.Operations.Pages.Sitemaps;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree;
using BetterCms.Module.Api.Operations.Pages.Widgets;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget.Options;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget.Options;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Tree;
using BetterCms.Module.Api.Operations.Root.CategorizableItems;
using BetterCms.Module.Api.Operations.Root.Languages;
using BetterCms.Module.Api.Operations.Root.Languages.Language;
using BetterCms.Module.Api.Operations.Root.Layouts;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Options;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions;
using BetterCms.Module.Api.Operations.Root.Tags;
using BetterCms.Module.Api.Operations.Root.Tags.Tag;
using BetterCms.Module.Api.Operations.Root.Version;
using BetterCms.Module.Api.Operations.Users;
using BetterCms.Module.Api.Operations.Users.Roles;
using BetterCms.Module.Api.Operations.Users.Roles.Role;
using BetterCms.Module.Api.Operations.Users.Users;
using BetterCms.Module.Api.Operations.Users.Users.User;
using BetterCms.Module.Api.Operations.Users.Users.User.Validate;
using BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser;

using BetterModules.Core.Modules.Registration;

using ContentService = BetterCms.Module.Api.Operations.Pages.Contents.Content.ContentService;

using INodeService = BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node.INodeService;
using INodesService = BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.INodesService;
using ISitemapService = BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.ISitemapService;
using ISitemapTreeService = BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree.ISitemapTreeService;

namespace BetterCms.Module.Api
{
    /// <summary>
    /// API module descriptor.
    /// </summary>
    public class ApiModuleDescriptor : CmsModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiModuleDescriptor" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public ApiModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
        }

        /// <summary>
        /// Gets the name of module.
        /// </summary>
        /// <value>
        /// The name of API module.
        /// </value>
        public override string Name
        {
            get
            {
                return "api";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The module description.
        /// </value>
        public override string Description
        {
            get
            {
                return "An API module for Better CMS.";
            }
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public override int Order
        {
            get
            {
                return int.MaxValue - 100;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<AuthorsController>().As<IAuthorsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<AuthorController>().As<IAuthorService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<BlogPostsSettingsController>().As<IBlogPostsSettingsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<BlogPostsController>().As<IBlogPostsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<BlogPostController>().As<IBlogPostService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<BlogPostPropertiesController>().As<IBlogPostPropertiesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            containerBuilder.RegisterType<FoldersController>().As<IFoldersService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<FolderController>().As<IFolderService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<FilesController>().As<IFilesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<FileController>().As<IFileService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<UploadFileService>().As<IUploadFileService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ImagesController>().As<IImagesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ImageController>().As<IImageService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<UploadImageService>().As<IUploadImageService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<MediaTreeController>().As<IMediaTreeService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            containerBuilder.RegisterType<CategoryTreesController>().As<ICategoryTreesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<CategoryTreeController>().As<ICategoryTreeService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<NodesTreeController>().As<INodesTreeService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<Operations.Root.Categories.Category.Nodes.CategoryNodesController>().As<Operations.Root.Categories.Category.Nodes.INodesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<Operations.Root.Categories.Category.Nodes.Node.CategoryNodeController>().As<Operations.Root.Categories.Category.Nodes.Node.INodeService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<CategorizableItemsController>().As<ICategorizableItemsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            containerBuilder.RegisterType<LanguagesController>().As<ILanguagesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<LanguageController>().As<ILanguageService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<TagsController>().As<ITagsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<TagController>().As<ITagService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<LayoutController>().As<ILayoutService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<LayoutsController>().As<ILayoutsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<LayoutRegionsController>().As<ILayoutRegionsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<LayoutOptionsController>().As<ILayoutOptionsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<VersionController>().As<IVersionService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            containerBuilder.RegisterType<PagesController>().As<IPagesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PageController>().As<IPageService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PageExistsController>().As<IPageExistsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PagePropertiesController>().As<IPagePropertiesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PageContentsController>().As<IPageContentsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PageContentController>().As<IPageContentService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PageContentOptionsController>().As<IPageContentOptionsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PageTranslationsController>().As<IPageTranslationsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<DefaultSearchPagesService>().As<ISearchPagesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            containerBuilder.RegisterType<RedirectsController>().As<IRedirectsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<RedirectController>().As<IRedirectService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);            
            containerBuilder.RegisterType<ContentService>().As<IContentService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ContentDraftController>().As<IContentDraftService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<BlogPostContentController>().As<IBlogPostContentService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<HtmlContentController>().As<IHtmlContentService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ContentHistoryController>().As<IContentHistoryService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<WidgetService>().As<IWidgetService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<WidgetsController>().As<IWidgetsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<HtmlContentWidgetController>().As<IHtmlContentWidgetService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<HtmlContentWidgetOptionsController>().As<IHtmlContentWidgetOptionsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ServerControlController>().As<IServerControlWidgetService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ServerControlWidgetOptionsController>().As<IServerControlWidgetOptionsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<SitemapsController>().As<ISitemapsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<SitemapController>().As<ISitemapService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<SitemapTreeController>().As<ISitemapTreeService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<NodesController>().As<INodesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<NodeController>().As<INodeService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            containerBuilder.RegisterType<DefaultRootApiOperations>().As<IRootApiOperations>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<DefaultPagesApiOperations>().As<IPagesApiOperations>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<DefaultBlogApiOperations>().As<IBlogApiOperations>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<DefaultMediaManagerApiOperations>().As<IMediaManagerApiOperations>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<DefaultApiFacade>().As<IApiFacade>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            containerBuilder.RegisterType<DefaultUsersApiOperations>().As<IUsersApiOperations>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<DefaultUsersService>().As<IUsersService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<DefaultUserService>().As<IUserService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<DefaultValidateUserService>().As<IValidateUserService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<DefaultRolesService>().As<IRolesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<DefaultRoleService>().As<IRoleService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }
    }
}
