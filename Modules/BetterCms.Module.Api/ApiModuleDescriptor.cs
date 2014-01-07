using Autofac;

using BetterCms.Core.Modules;

using BetterCms.Module.Api.Operations.Blog;
using BetterCms.Module.Api.Operations.Blog.Authors;
using BetterCms.Module.Api.Operations.Blog.Authors.Author;
using BetterCms.Module.Api.Operations.Blog.BlogPosts;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Content;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Api.Operations.MediaManager;
using BetterCms.Module.Api.Operations.MediaManager.Files;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.MediaManager.Images;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.Api.Operations.MediaManager.MediaTree;
using BetterCms.Module.Api.Operations.Pages;
using BetterCms.Module.Api.Operations.Pages.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent;
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
using BetterCms.Module.Api.Operations.Pages.Sitemap;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Tree;
using BetterCms.Module.Api.Operations.Pages.Widgets;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget.Options;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
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

using ContentService = BetterCms.Module.Api.Operations.Pages.Contents.Content.ContentService;

namespace BetterCms.Module.Api
{
    /// <summary>
    /// API module descriptor.
    /// </summary>
    public class ApiModuleDescriptor : ModuleDescriptor
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
            containerBuilder.RegisterType<AuthorsService>().As<IAuthorsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<AuthorService>().As<IAuthorService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<BlogPostsService>().As<IBlogPostsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<BlogPostService>().As<IBlogPostService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<BlogPostPropertiesService>().As<IBlogPostPropertiesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            
            containerBuilder.RegisterType<FilesService>().As<IFilesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ImagesService>().As<IImagesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ImageService>().As<IImageService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<MediaTreeService>().As<IMediaTreeService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            containerBuilder.RegisterType<CategoriesService>().As<ICategoriesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<LanguagesService>().As<ILanguagesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<TagsService>().As<ITagsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<TagService>().As<ITagService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<LayoutService>().As<ILayoutService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<LayoutsService>().As<ILayoutsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<LayoutRegionsService>().As<ILayoutRegionsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<LayoutOptionsService>().As<ILayoutOptionsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<VersionService>().As<IVersionService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            containerBuilder.RegisterType<PagesService>().As<IPagesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PageService>().As<IPageService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PageExistsService>().As<IPageExistsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PagePropertiesService>().As<IPagePropertiesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PageContentsService>().As<IPageContentsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PageContentService>().As<IPageContentService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PageContentOptionsService>().As<IPageContentOptionsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<PageTranslationsService>().As<IPageTranslationsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<DefaultSearchPagesService>().As<ISearchPagesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            containerBuilder.RegisterType<RedirectsService>().As<IRedirectsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<RedirectService>().As<IRedirectService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);            
            containerBuilder.RegisterType<SitemapService>().As<ISitemapService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ContentService>().As<IContentService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<BlogPostContentService>().As<IBlogPostContentService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<HtmlContentService>().As<IHtmlContentService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ContentHistoryService>().As<IContentHistoryService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<WidgetService>().As<IWidgetService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);            
            containerBuilder.RegisterType<WidgetsService>().As<IWidgetsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<HtmlContentWidgetService>().As<IHtmlContentWidgetService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ServerControlWidgetService>().As<IServerControlWidgetService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ServerControlWidgetOptionsService>().As<IServerControlWidgetOptionsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<SitemapTreeService>().As<ISitemapTreeService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<NodesService>().As<INodesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<NodeService>().As<INodeService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

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
        }
    }
}
