using System;
using System.Collections.Generic;

using Autofac;

using BetterCms.Core;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Modules.Registration;
using BetterCms.Events;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.Registration;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Root
{
    /// <summary>
    /// Root functionality module descriptor.
    /// </summary>
    public class RootModuleDescriptor : ModuleDescriptor
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
        /// bcms.languages.js java script module descriptor.
        /// </summary>
        private readonly LanguagesJsModuleIncludeDescriptor languagesJsModuleIncludeDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootModuleDescriptor" /> class.
        /// </summary>
        public RootModuleDescriptor(ICmsConfiguration configuration) : base(configuration)
        {    
            authenticationJsModuleIncludeDescriptor = new AuthenticationJsModuleIncludeDescriptor(this);
            siteSettingsJsModuleIncludeDescriptor = new SiteSettingsJsModuleIncludeDescriptor(this);
            tagsJsModuleIncludeDescriptor = new TagsJsModuleIncludeDescriptor(this);
            languagesJsModuleIncludeDescriptor = new LanguagesJsModuleIncludeDescriptor(this);

            InitializeSecurity();            
        }

        internal const string ModuleId = "456353c3-f4af-4016-838b-12e4677c3133";

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public override Guid Id
        {
            get
            {
                return new Guid(ModuleId);
            }
        }

        /// <summary>
        /// Flag describe is module root or additional
        /// </summary>
        public override bool IsRootModule
        {
            get
            {
                return true;
            }
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
        public override string AreaName
        {
            get
            {
                return RootAreaName;
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
                return RootSchemaName;
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
                return int.MaxValue;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DefaultSecurityService>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<PageContentProjectionFactory>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultContentService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultRenderingService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<PageStylesheetProjectionFactory>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterType<PageJavaScriptProjectionFactory>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultOptionService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultAccessControlService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultEntityTrackingService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultEntityTrackingCacheService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultLanguageService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultContentProjectionService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultChildContentService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers a routes.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public override void RegisterCustomRoutes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {            
            context.MapRoute(
                "bcms_" + AreaName + "_MainJs",
                string.Format(RootModuleConstants.AutoGeneratedJsFilePathPattern, "bcms.main.js").TrimStart('/'),
                new
                    {
                        area = AreaName,
                        controller = "Rendering",
                        action = "RenderMainJsFile"
                    },
                new[] { typeof(RenderingController).Namespace });
           
            context.MapRoute(
                "bcms_" + AreaName + "_ProcessorJs",
                string.Format(RootModuleConstants.AutoGeneratedJsFilePathPattern, "bcms.processor.js").TrimStart('/'),                
                new
                    {
                        area = AreaName,
                        controller = "Rendering",
                        action = "RenderProcessorJsFile"
                    },  
                new[] { typeof(RenderingController).Namespace });            
            
            context.MapRoute(
                "bcms_" + AreaName + "_PreviewPage",
                "bcms/preview/{pageId}/{pageContentId}",
                new
                {
                    area = AreaName,
                    controller = "Preview",
                    action = "Index"
                },
                new[] { typeof(PreviewController).Namespace });


            CreateEmbeddedResourcesRoutes(context);

            // All CMS Pages:
            context.MapRoute("bcms_" + AreaName + "_AllPages", 
                "{*data}", 
                new
                    {
                        area = AreaName, 
                        controller = "Cms", 
                        action = "Index"
                    }, 
                    new[] { typeof(CmsController).Namespace });
        }

        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[]
                       {
                           new CssIncludeDescriptor(this, "base.css"),
                           new CssIncludeDescriptor(this, "bcms.messages.css")
                       };
        }

        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new []
                {
                    authenticationJsModuleIncludeDescriptor,                    
                    new ContentJsModuleIncludeDescriptor(this),       
                    new ContentTreeJsModuleIncludeDescriptor(this),
                    new DatePickerJsModuleIncludeDescriptor(this), 
                    new DynamicContentJsModuleIncludeDescriptor(this), 
                    new FormsJsModuleIncludeDescriptor(this),
                    new JsIncludeDescriptor(this, "bcms.grid"), 
                    new HtmlEditorJsModuleIncludeDescriptor(this),                     
                    new CodeEditorJsModuleIncludeDescriptor(this),                     
                    new InlineEditorJsModuleIncludeDescriptor(this),
                    new JsIncludeDescriptor(this, "bcms.jquery", "bcms.jquery-1.7.2"),
                    new JsIncludeDescriptor(this, "bcms.jqueryui", "bcms.jquery-ui-1.9.0"),
                    new JsIncludeDescriptor(this, "bcms.jquery.unobtrusive", "bcms.jquery.unobtrusive-ajax"),
                    new JsIncludeDescriptor(this, "bcms.jquery.validate"),
                    new JsIncludeDescriptor(this, "bcms.jquery.validate.unobtrusive"),
                    new JsIncludeDescriptor(this, "bcms.jquery.autocomplete"),
                    new JsIncludeDescriptor(this, "bcms.autocomplete"),
                    new BcmsJsModuleIncludeDescriptor(this), 
                    new KnockoutExtendersJsModuleIncludeDescriptor(this), 
                    new JsIncludeDescriptor(this, "bcms.ko.grid"),                    
                    new SecurityJsModuleIncludeDescriptor(this), 
                    new MessagesJsModuleIncludeDescriptor(this), 
                    new ModalJsModuleIncludeDescriptor(this), 
                    new PreviewJsModuleIncludeDescriptor(this), 
                    new RedirectJsModuleIncludeDescriptor(this),
                    new SidemenuJsModuleIncludeDescriptor(this),                     
                    siteSettingsJsModuleIncludeDescriptor,
                    new JsIncludeDescriptor(this, "bcms.slides.jquery"),
                    new JsIncludeDescriptor(this, "bcms.spinner.jquery"),
                    new TabsJsModuleIncludeDescriptor(this), 
                    new TooltipJsModuleIncludeDescriptor(this),                    
                    new JsIncludeDescriptor(this,"bcms.processor", isAutoGenerated:true),
                    new JsIncludeDescriptor(this, "knockout", "bcms.knockout-2.2.1.js", "bcms.knockout-2.2.1.min.js"), 
                    new JsIncludeDescriptor(this, "ace", "ace/ace.js", "ace/ace.js"),
                    new JsIncludeDescriptor(this, "ckeditor", "ckeditor/ckeditor.js", "ckeditor/ckeditor.js"),
                    tagsJsModuleIncludeDescriptor,
                    languagesJsModuleIncludeDescriptor,
                    new OptionsJsModuleIncludeDescriptor(this)
                };
        }

        public override IEnumerable<IPageActionProjection> RegisterSidebarHeaderProjections(ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
                {
                    new ButtonActionProjection(authenticationJsModuleIncludeDescriptor, page => RootGlobalization.Sidebar_LogoutButton, page => "logout")
                        {
                            Order = 10,
                            CssClass = page => "bcms-logout-btn",
                        },
                    new RenderActionProjection<AuthenticationController>(f => f.Info())
                };
        }

        public override IEnumerable<IPageActionProjection> RegisterSidebarMainProjections(ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
                {                    
                    new ButtonActionProjection(siteSettingsJsModuleIncludeDescriptor, page => "openSiteSettings")
                        {
                            Title = page => RootGlobalization.Sidebar_SiteSettingsButtonTitle,
                            CssClass = page => "bcms-sidemenu-btn bcms-btn-settings",
                            Order = 500,
                        }
                };
        }

        /// <summary>
        /// Registers the site settings projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns>Settings action projections.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder)
        {
            return new List<IPageActionProjection>
                {
                    new LinkActionProjection(tagsJsModuleIncludeDescriptor, page => "loadSiteSettingsCategoryList")
                        {
                            Order = 2000,
                            Title = page => RootGlobalization.SiteSettings_CategoriesMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.EditContent
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
                            ShouldBeRendered = page => CmsContext.Config.EnableMultilanguage
                        }
                };
        }

        /// <summary>
        /// Creates the resource routes for 6 levels folder structure.
        /// </summary>
        /// <param name="context">The context.</param>
        private void CreateEmbeddedResourcesRoutes(ModuleRegistrationContext context)
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

            int i = 0;
            foreach (var url in urls)
            {
                context.MapRoute(
                    AreaName + "-level" + i++,
                    url,
                    new
                    {
                        controller = "EmbeddedResources",
                        action = "Index"
                    },
                    new
                    {
                        resourceType = new MimeTypeRouteConstraint()
                    },
                    new[] { typeof(EmbeddedResourcesController).Namespace });
            }
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
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                if (args.Session == null || args.Session.IsDirtyEntity(args.Entity))
                {
                    var tracker = container.Resolve<IEntityTrackingService>();
                    tracker.OnEntityUpdate(args.Entity);
                }
            }
        }

        private void OnEntityDelete(SingleItemEventArgs<IEntity> args)
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var tracker = container.Resolve<IEntityTrackingService>();
                tracker.OnEntityDelete(args.Item);
            }
        }
    }
}
