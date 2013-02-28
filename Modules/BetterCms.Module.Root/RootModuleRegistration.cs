using System.Collections.Generic;

using Autofac;

using BetterCms.Core.Models;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Security;
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
        internal const string ModuleName = "root";

        private AuthenticationScriptModuleDescriptor authenticationScriptModuleDescriptor;

        private SiteSettingsJavaScriptModuleDescriptor siteSettingsJavaScriptModuleDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootModuleDescriptor" /> class.
        /// </summary>
        public RootModuleDescriptor()
        {    
            authenticationScriptModuleDescriptor = new AuthenticationScriptModuleDescriptor(this);
            siteSettingsJavaScriptModuleDescriptor = new SiteSettingsJavaScriptModuleDescriptor(this);
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
                return "Root functionality module for BetterCMS.";
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
        /// <param name="configuration">The configuration.</param>
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            containerBuilder.RegisterType<DefaultSecurityService>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<PageContentProjectionFactory>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultContentService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<PageStylesheetProjectionFactory>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterType<PageJavaScriptProjectionFactory>().AsSelf().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers a routes.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        public override void RegisterCustomRoutes(ModuleRegistrationContext context, ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {            
            context.MapRoute(
                "bcms_" + AreaName + "_MainJs",
                string.Format("file/{0}/Scripts/main.js", AreaName),
                new
                    {
                        area = AreaName,
                        controller = "Rendering",
                        action = "RenderMainJsFile"
                    },
                new[] { typeof(RenderingController).Namespace });

            context.MapRoute(
                "bcms_" + AreaName + "_ProcessorJs",
                string.Format("file/{0}/Scripts/bcms.processor.js", AreaName),
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

        public override IEnumerable<JavaScriptModuleDescriptor> RegisterJavaScriptModules(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new []
                {
                    authenticationScriptModuleDescriptor,
                    siteSettingsJavaScriptModuleDescriptor,
                    new ContentJavaScriptModuleDescriptor(this),       
                    new DatePickerJavaScriptModuleDescriptor(this), 
                    new DynamicContentJavaScriptModuleDescriptor(this), 
                    new FormsJavaScriptModuleDescriptor(this),
                    new HtmlEditorJavaScriptModuleDescriptor(this),                     
                    new MessagesJavaScriptModuleDescriptor(this), 
                    new ModalJavaScriptModuleDescriptor(this), 
                    new SidemenuJavaScriptModuleDescriptor(this),                     
                    new TabsJavaScriptModuleDescriptor(this), 
                    new TooltipJavaScriptModuleDescriptor(this),
                    new InlineEditorJavaScriptModuleDescriptor(this),
                    new PreviewJavaScriptModuleDescriptor(this), 
                    new JavaScriptModuleDescriptor(this, "bcms.ko.grid", "/file/bcms-root/scripts/bcms.ko.grid"),
                    new JavaScriptModuleDescriptor(this, "bcms.ko.extenders", "/file/bcms-root/scripts/bcms.ko.extenders"),
                    new RedirectJavaScriptModuleDescriptor(this),
                };
        }

        public override IEnumerable<IUserRole> RegisterUserRoles(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new[] { new UserRole(UserRoles.EditSiteSettings, RootGlobalization.UserRole_EditSiteSettings) };
        }

        public override IEnumerable<IPageActionProjection> RegisterSidebarHeaderProjections(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new IPageActionProjection[]
                {
                    new ButtonActionProjection(authenticationScriptModuleDescriptor, () => RootGlobalization.Sidebar_LogoutButton, page => "logout")
                        {
                            Order = 10,
                            CssClass = page => "bcms-logout-btn",
                            IsVisible = (page, principal) => principal.Identity.IsAuthenticated
                        },
                    new RenderActionProjection<AuthenticationController>(f => f.Info()) { IsVisible = (page, principal) => principal.Identity.IsAuthenticated }
                };
        }

        public override IEnumerable<IPageActionProjection> RegisterSidebarMainProjections(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new IPageActionProjection[]
                {                    
                    new ButtonActionProjection(siteSettingsJavaScriptModuleDescriptor, page => "openSiteSettings")
                        {
                            Title = () => RootGlobalization.Sidebar_SiteSettingsButtonTitle,
                            CssClass = page => "bcms-sidemenu-btn bcms-btn-settings",
                            Order = 500,
                            IsVisible = (page, principal) => principal.IsInRole(UserRoles.EditSiteSettings)
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
    }
}
