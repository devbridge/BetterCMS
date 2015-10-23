using System.Collections.Generic;
using BetterCms.Configuration;
using BetterCms.Core;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Accessors;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;
using BetterCms.Module.Root.Models.Accessors;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.Registration;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.Services.Categories.Nodes;
using BetterCms.Module.Root.Services.Categories.Tree;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataContracts;
using BetterModules.Core.Modules.Registration;
using BetterModules.Core.Web.Modules.Registration;
using BetterModules.Core.Web.Mvc.Extensions;
using BetterModules.Events;
using Microsoft.AspNet.Authorization;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;

namespace BetterCms.Module.Root
{
    /// <summary>
    /// Root functionality module descriptor.
    /// </summary>
    public class RootModuleDescriptor : CmsModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "root";

        /// <summary>
        /// The root area name.
        /// </summary>
        internal const string RootAreaName = "bcms-root";

        /// <summary>
        /// The root module database schema name
        /// </summary>
        internal const string RootSchemaName = "bcms_root";

        /// <summary>
        /// The bcms.authentication.js include descriptor
        /// </summary>
        private readonly AuthenticationJsModuleIncludeDescriptor authenticationJsModuleIncludeDescriptor;

        /// <summary>
        /// The bcms.siteSettings.js include descriptor
        /// </summary>
        private readonly SiteSettingsJsModuleIncludeDescriptor siteSettingsJsModuleIncludeDescriptor;

        /// <summary>
        /// bcms.tags.js java script module descriptor.
        /// </summary>
        private readonly TagsJsModuleIncludeDescriptor tagsJsModuleIncludeDescriptor;

        /// <summary>
        /// bcms.categories.js java script module descriptor.
        /// </summary>
        private readonly CategoriesJavaScriptModuleDescriptor categoriesJavaScriptModuleDescriptor;

        /// <summary>
        /// bcms.languages.js java script module descriptor.
        /// </summary>
        private readonly LanguagesJsModuleIncludeDescriptor languagesJsModuleIncludeDescriptor;

        private readonly ILoggerFactory loggerFactory;

        private readonly IControllerExtensions controllerExtensions;

        private readonly IEntityTrackingService entityTrackingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootModuleDescriptor" /> class.
        /// </summary>
        public RootModuleDescriptor(IOptions<CmsConfigurationSection> configuration, ILoggerFactory loggerFactory,
            IControllerExtensions controllerExtensions, IEntityTrackingService entityTrackingService) : base(configuration)
        {
            this.loggerFactory = loggerFactory;
            this.controllerExtensions = controllerExtensions;
            this.entityTrackingService = entityTrackingService;
            authenticationJsModuleIncludeDescriptor = new AuthenticationJsModuleIncludeDescriptor(this, loggerFactory, controllerExtensions);
            siteSettingsJsModuleIncludeDescriptor = new SiteSettingsJsModuleIncludeDescriptor(this, loggerFactory, controllerExtensions);
            tagsJsModuleIncludeDescriptor = new TagsJsModuleIncludeDescriptor(this, loggerFactory, controllerExtensions);
            categoriesJavaScriptModuleDescriptor = new CategoriesJavaScriptModuleDescriptor(this, loggerFactory, controllerExtensions);
            languagesJsModuleIncludeDescriptor = new LanguagesJsModuleIncludeDescriptor(this, loggerFactory, controllerExtensions);
            CategoryAccessors.Register<WidgetCategoryAccessor>();
            InitializeSecurity();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description
        {
            get
            {
                return "Root functionality module for Better CMS.";
            }
        }

        /// <summary>
        /// Gets the name of the module area.
        /// </summary>
        /// <value>
        /// The name of the module area.
        /// </value>
        public override string AreaName => RootAreaName;

        /// <summary>
        /// Gets the name of the module database schema name.
        /// </summary>
        /// <value>
        /// The name of the module database schema.
        /// </value>
        public override string SchemaName => RootSchemaName;

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public override int Order => int.MaxValue;

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="services">The collection of services</param>
        public override void RegisterModuleTypes(ModuleRegistrationContext context, IServiceCollection services)
        {
            //containerBuilder.RegisterType<DefaultSecurityService>().AsImplementedInterfaces().SingleInstance();
            //containerBuilder.RegisterType<PageContentProjectionFactory>().AsSelf().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DefaultContentService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DefaultRenderingService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<PageStylesheetProjectionFactory>().AsSelf().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<PageJavaScriptProjectionFactory>().AsSelf().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DefaultOptionService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DefaultAccessControlService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DefaultEntityTrackingService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DefaultEntityTrackingCacheService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DefaultLanguageService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DefaultContentProjectionService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DefaultChildContentService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DefaultCategoryService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DefaultCategoryTreeService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DefaultCategoryNodeService>().AsImplementedInterfaces().InstancePerLifetimeScope();

            services.AddSingleton<ISecurityService, DefaultSecurityService>();
            services.AddScoped<PageContentProjectionFactory>();
            services.AddScoped<IContentService, DefaultContentService>();
            services.AddScoped<IRenderingService, DefaultRenderingService>();
            services.AddScoped<PageStylesheetProjectionFactory>();
            services.AddScoped<PageJavaScriptProjectionFactory>();
            services.AddScoped<IOptionService, DefaultOptionService>();
            services.AddScoped<IAccessControlService, DefaultAccessControlService>();
            services.AddScoped<IEntityTrackingService, DefaultEntityTrackingService>();
            services.AddScoped<IEntityTrackingCacheService, DefaultEntityTrackingCacheService>();
            services.AddScoped<ILanguageService, DefaultLanguageService>();
            services.AddScoped<IContentProjectionService, DefaultContentProjectionService>();
            services.AddScoped<IChildContentService, DefaultChildContentService>();
            services.AddScoped<ICategoryService, DefaultCategoryService>();
            services.AddScoped<ICategoryTreeService, DefaultCategoryTreeService>();
            services.AddScoped<ICategoryNodeService, DefaultCategoryNodeService>();
        }

        ///// <summary>
        ///// Registers a routes.
        ///// </summary>
        ///// <param name="context">The area registration context.</param>
        ///// <param name="containerBuilder">The container builder.</param>
        //public override void RegisterCustomRoutes(WebModuleRegistrationContext context, ContainerBuilder containerBuilder)
        //{            
        //    context.MapRoute(
        //        "bcms_" + AreaName + "_MainJs",
        //        string.Format(RootModuleConstants.AutoGeneratedJsFilePathPattern, "bcms.main.js").TrimStart('/'),
        //        new
        //            {
        //                area = AreaName,
        //                controller = "Rendering",
        //                action = "RenderMainJsFile"
        //            },
        //        new[] { typeof(RenderingController).Namespace });

        //    context.MapRoute(
        //        "bcms_" + AreaName + "_ProcessorJs",
        //        string.Format(RootModuleConstants.AutoGeneratedJsFilePathPattern, "bcms.processor.js").TrimStart('/'),                
        //        new
        //            {
        //                area = AreaName,
        //                controller = "Rendering",
        //                action = "RenderProcessorJsFile"
        //            },  
        //        new[] { typeof(RenderingController).Namespace });            

        //    context.MapRoute(
        //        "bcms_" + AreaName + "_PreviewPage",
        //        "bcms/preview/{pageId}/{pageContentId}",
        //        new
        //        {
        //            area = AreaName,
        //            controller = "Preview",
        //            action = "Index"
        //        },
        //        new[] { typeof(PreviewController).Namespace });


        //    CreateEmbeddedResourcesRoutes(context);

        //    // All CMS Pages:
        //    context.MapRoute("bcms_" + AreaName + "_AllPages", 
        //        "{*data}", 
        //        new
        //            {
        //                area = AreaName, 
        //                controller = "Cms", 
        //                action = "Index"
        //            }, 
        //            new[] { typeof(CmsController).Namespace });
        //}

        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[]
                       {
                           new CssIncludeDescriptor(this, "bcms.root.css")
                       };
        }

        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new[]
                {
                    authenticationJsModuleIncludeDescriptor,
                    new ContentJsModuleIncludeDescriptor(this, loggerFactory),
                    new ContentTreeJsModuleIncludeDescriptor(this, loggerFactory),
                    new DatePickerJsModuleIncludeDescriptor(this, loggerFactory, controllerExtensions),
                    new DynamicContentJsModuleIncludeDescriptor(this, loggerFactory),
                    new FormsJsModuleIncludeDescriptor(this),
                    new JsIncludeDescriptor(this, "bcms.grid"),
                    new HtmlEditorJsModuleIncludeDescriptor(this),
                    new CodeEditorJsModuleIncludeDescriptor(this),
                    new InlineEditorJsModuleIncludeDescriptor(this, loggerFactory),
                    new JsIncludeDescriptor(this, "bcms.jquery", "bcms.jquery-1.7.2"),
                    new JsIncludeDescriptor(this, "bcms.jqueryui", "bcms.jquery-ui-1.9.0"),
                    new JsIncludeDescriptor(this, "bcms.jquery.unobtrusive", "bcms.jquery.unobtrusive-ajax"),
                    new JsIncludeDescriptor(this, "bcms.jquery.validate"),
                    new JsIncludeDescriptor(this, "bcms.jquery.validate.unobtrusive"),
                    new JsIncludeDescriptor(this, "bcms.jquery.autocomplete"),
                    new JsIncludeDescriptor(this, "bcms.autocomplete"),
                    new JsIncludeDescriptor(this, "bcms.multiple.select"),
                    new AntiXssJsModuleIncludeDescriptor(this, loggerFactory),
                    new CustomValidationJsModuleIncludeDescriptor(this),
                    new BcmsJsModuleIncludeDescriptor(this, loggerFactory),
                    new KnockoutExtendersJsModuleIncludeDescriptor(this, loggerFactory),
                    new JsIncludeDescriptor(this, "bcms.ko.grid"),
                    new SecurityJsModuleIncludeDescriptor(this, loggerFactory, controllerExtensions),
                    new MessagesJsModuleIncludeDescriptor(this),
                    new ModalJsModuleIncludeDescriptor(this, loggerFactory),
                    new PreviewJsModuleIncludeDescriptor(this, loggerFactory, controllerExtensions),
                    new RedirectJsModuleIncludeDescriptor(this, loggerFactory),
                    new SidemenuJsModuleIncludeDescriptor(this, loggerFactory),
                    siteSettingsJsModuleIncludeDescriptor,
                    new JsIncludeDescriptor(this, "bcms.slides.jquery"),
                    new JsIncludeDescriptor(this, "bcms.spinner.jquery"),
                    new JsIncludeDescriptor(this, "bcms.store"),
                    new TabsJsModuleIncludeDescriptor(this),
                    new TooltipJsModuleIncludeDescriptor(this),
                    new JsIncludeDescriptor(this,"bcms.processor", isAutoGenerated:true),
                    new JsIncludeDescriptor(this, "knockout", "bcms.knockout-2.2.1.js", "bcms.knockout-2.2.1.min.js"),
                    new JsIncludeDescriptor(this, "ace", "ace/ace.js", "ace/ace.js"),
                    new JsIncludeDescriptor(this, "ckeditor", "ckeditor/ckeditor.js", "ckeditor/ckeditor.js"),
                    tagsJsModuleIncludeDescriptor,
                    categoriesJavaScriptModuleDescriptor,
                    languagesJsModuleIncludeDescriptor,
                    new OptionsJsModuleIncludeDescriptor(this, loggerFactory)
                };
        }

        public override IEnumerable<IPageActionProjection> RegisterSidebarHeaderProjections(IServiceCollection services)
        {
            return new IPageActionProjection[]
                {
                    new ButtonActionProjection(authenticationJsModuleIncludeDescriptor, page => RootGlobalization.Sidebar_LogoutButton, page => "logout")
                        {
                            Order = 10,
                            CssClass = page => "bcms-btn-logout",
                        },
                    // TODO move Info action to view component and register RenderViewComponentProjection
                    //new RenderActionProjection<AuthenticationController>(f => f.Info())
                };
        }

        public override IEnumerable<IPageActionProjection> RegisterSidebarMainProjections(IServiceCollection services)
        {
            return new IPageActionProjection[]
                {
                    new ButtonActionProjection(siteSettingsJsModuleIncludeDescriptor, page => "openSiteSettings")
                        {
                            Title = page => RootGlobalization.Sidebar_SiteSettingsButtonTitle,
                            CssClass = page => "bcms-sidemenu-btn bcms-btn-settings",
                            Order = 900,
                        }
                };
        }

        /// <summary>
        /// Registers the site settings projections.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>Settings action projections.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(IServiceCollection services)
        {
            return new List<IPageActionProjection>
                {
                    new LinkActionProjection(categoriesJavaScriptModuleDescriptor, page => "loadSiteSettingsCategoryTreesList")
                        {
                            Order = 2000,
                            Title = page => RootGlobalization.SiteSettings_CategoriesMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)
                        },
                   new LinkActionProjection(tagsJsModuleIncludeDescriptor, page => "loadSiteSettingsTagList")
                        {
                            Order = 2100,
                            Title = page => RootGlobalization.SiteSettings_TagsMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.EditContent
                        },
                    new LinkActionProjection(languagesJsModuleIncludeDescriptor, page => "loadSiteSettingsLanguagesList")
                        {
                            Order = 2200,
                            Title = page => RootGlobalization.SiteSettings_LanguagesMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.Administration,
                            ShouldBeRendered = page => Configuration.EnableMultilanguage
                        }
                };
        }

        public override void RegisterAuthorizationPolicies(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var securityService = provider.GetService<ISecurityService>();

            services.Configure<AuthorizationOptions>(options =>
            {
                options.AddPolicy(RootModuleConstants.Policies.AdministrationOnly, builder =>
                {
                    builder.AddRequirements(
                        new BcmsAuthorizeRequirement(securityService, RootModuleConstants.UserRoles.Administration));
                });
                options.AddPolicy(RootModuleConstants.Policies.CanEditContentOrAdmin, builder =>
                {
                    builder.AddRequirements(
                        new BcmsAuthorizeRequirement(securityService, RootModuleConstants.UserRoles.Administration, RootModuleConstants.UserRoles.EditContent));
                });
                options.AddPolicy(RootModuleConstants.Policies.CanEditContent, builder =>
                {
                    builder.AddRequirements(
                        new BcmsAuthorizeRequirement(securityService, RootModuleConstants.UserRoles.Administration));
                });
            });
        }

        /// <summary>
        /// Creates the resource routes for 6 levels folder structure.
        /// </summary>
        /// <param name="context">The context.</param>
        private void CreateEmbeddedResourcesRoutes(WebModuleRegistrationContext context)
        {
            string[] urls = new[]
                {
                    "file/{area}/{file}.{resourceType}/{*all}",
                    "file/{area}/{folder1}/{file}.{resourceType}/{*all}",
                    "file/{area}/{folder1}/{folder2}/{file}.{resourceType}/{*all}",
                    "file/{area}/{folder1}/{folder2}/{folder3}/{file}.{resourceType}/{*all}",
                    "file/{area}/{folder1}/{folder2}/{folder3}/{folder4}/{file}.{resourceType}/{*all}",
                    "file/{area}/{folder1}/{folder2}/{folder3}/{folder4}/{folder5}/{file}.{resourceType}/{*all}",
                    "file/{area}/{folder1}/{folder2}/{folder3}/{folder4}/{folder5}/{folder6}/{file}.{resourceType}/{*all}"
                };

            //int i = 0;
            //foreach (var url in urls)
            //{
            //    context.MapRoute(
            //        AreaName + "-level" + i++,
            //        url,
            //        new
            //        {
            //            controller = "EmbeddedResources",
            //            action = "Index"
            //        },
            //        new
            //        {
            //            resourceType = new MimeTypeRouteConstraint()
            //        },
            //        new[] { typeof(EmbeddedResourcesController).Namespace });
            //}
        }

        private void InitializeSecurity()
        {
            if (Configuration.Security.AccessControlEnabled)
            {
                CoreEvents.Instance.EntitySaving += OnEntitySave;
                CoreEvents.Instance.EntityDeleting += OnEntityDelete;
            }
        }

        private void OnEntitySave(EntitySavingEventArgs args)
        {
            if (args.Session == null || args.Session.IsDirtyEntity(args.Entity))
            {
                entityTrackingService.OnEntityUpdate(args.Entity);
            }
        }

        private void OnEntityDelete(SingleItemEventArgs<IEntity> args)
        {
            entityTrackingService.OnEntityDelete(args.Item);
        }
    }
}
