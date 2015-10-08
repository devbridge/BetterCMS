using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Autofac;

using BetterCms.Core;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Events;

using BetterCms.Module.Pages.Accessors;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Helpers.Extensions;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Models.Accessors;
using BetterCms.Module.Pages.Mvc.PageHtmlRenderer;
using BetterCms.Module.Pages.Mvc.Projections;
using BetterCms.Module.Pages.Registration;
using BetterCms.Module.Pages.Services;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Accessors;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Cms;

using BetterModules.Core.Modules.Registration;

namespace BetterCms.Module.Pages
{
    /// <summary>
    /// Pages module descriptor.
    /// </summary>
    public class PagesModuleDescriptor : CmsModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "pages";

        /// <summary>
        /// The pages module area name
        /// </summary>
        internal const string PagesAreaName = "bcms-pages";

        /// <summary>
        /// The pages module database schema name
        /// </summary>
        internal const string PagesSchemaName = "bcms_pages";

        /// <summary>
        /// bcms.pages.js java script module descriptor.
        /// </summary>
        private readonly PagesJsModuleIncludeDescriptor pagesJsModuleIncludeDescriptor;

        /// <summary>
        /// bcms.pages.properties.js java script module descriptor.
        /// </summary>
        private readonly PagePropertiesJsModuleIncludeDescriptor pagePropertiesJsModuleIncludeDescriptor;

        /// <summary>
        /// bcms.pages.seo.js java script module descriptor.
        /// </summary>
        private readonly SeoJsModuleIncludeDescriptor seoJsModuleIncludeDescriptor;

        /// <summary>
        /// bcms.pages.content.js java script module descriptor.
        /// </summary>
        private readonly PagesContentJsModuleIncludeDescriptor pagesContentJsModuleIncludeDescriptor;

        /// <summary>
        /// bcms.pages.redirects.js java script module descriptor.
        /// </summary>
        private readonly RedirectsJsModuleIncludeDescriptor redirectsJsModuleIncludeDescriptor;

        /// <summary>
        /// bcms.pages.widgets.js java script module descriptor.
        /// </summary>
        private readonly WidgetsJsModuleIncludeDescriptor widgetsJsModuleIncludeDescriptor;

        /// <summary>
        /// bcms.pages.templates.js java script module descriptor.
        /// </summary>
        private readonly TemplatesJsModuleIncludeDescriptor templatesJsModuleIncludeDescriptor;

        /// <summary>
        /// bcms.pages.masterpage.js module include descriptor.
        /// </summary>
        private readonly MasterPagesJsModuleIncludeDescriptor masterPagesJsModuleIncludeDescriptor;

        /// <summary>
        /// bcms.pages.history.js java script module descriptor.
        /// </summary>
        private readonly HistoryJsModuleIncludeDescriptor historyJsModuleIncludeDescriptor;

        /// <summary>
        /// bcms.pages.sitemap.js java script module descriptor.
        /// </summary>
        private readonly SitemapJsModuleIncludeDescriptor sitemapJsModuleIncludeDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagesModuleDescriptor" /> class.
        /// </summary>
        public PagesModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
            pagesJsModuleIncludeDescriptor = new PagesJsModuleIncludeDescriptor(this);
            pagePropertiesJsModuleIncludeDescriptor = new PagePropertiesJsModuleIncludeDescriptor(this);
            seoJsModuleIncludeDescriptor = new SeoJsModuleIncludeDescriptor(this);
            pagesContentJsModuleIncludeDescriptor = new PagesContentJsModuleIncludeDescriptor(this);
            widgetsJsModuleIncludeDescriptor = new WidgetsJsModuleIncludeDescriptor(this);
            redirectsJsModuleIncludeDescriptor = new RedirectsJsModuleIncludeDescriptor(this);
            templatesJsModuleIncludeDescriptor = new TemplatesJsModuleIncludeDescriptor(this);
            masterPagesJsModuleIncludeDescriptor = new MasterPagesJsModuleIncludeDescriptor(this);
            historyJsModuleIncludeDescriptor = new HistoryJsModuleIncludeDescriptor(this);
            sitemapJsModuleIncludeDescriptor = new SitemapJsModuleIncludeDescriptor(this);
            //            CategoryAccessors.Register<PageCategory, PageProperties>(PageProperties.CategorizableItemKeyForPages);
            CategoryAccessors.Register<PageCategoryAccessor>();

            RootEvents.Instance.PageRetrieved += Events_PageRetrieved;

            RegisterRenderingPageProperties();
        }

        /// <summary>
        /// Gets the name of module.
        /// </summary>
        /// <value>
        /// The name of pages module.
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
        /// The module description.
        /// </value>
        public override string Description
        {
            get
            {
                return "Pages module for Better CMS.";
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
                return int.MaxValue - 300;
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
                return PagesAreaName;
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
                return PagesSchemaName;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            RegisterStylesheetRendererType<PageStylesheetAccessor, PageProperties>(containerBuilder);
            RegisterJavaScriptRendererType<PageJavaScriptAccessor, PageProperties>(containerBuilder);

            RegisterStylesheetRendererType<PageStylesheetAccessor, Root.Models.Page>(containerBuilder);
            RegisterJavaScriptRendererType<PageJavaScriptAccessor, Root.Models.Page>(containerBuilder);

            RegisterContentRendererType<HtmlContentAccessor, HtmlContent>(containerBuilder);
            RegisterContentRendererType<HtmlContentWidgetAccessor, HtmlContentWidget>(containerBuilder);
            RegisterContentRendererType<ServerControlWidgetAccessor, ServerControlWidget>(containerBuilder);

            containerBuilder.RegisterType<DefaultPageService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultRedirectService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultTagService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultHistoryService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultSitemapService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultUrlService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultLayoutService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultPreviewService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultMasterPageService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultPageCloneService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultWidgetService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultDraftService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultPageListService>().As<IPageListService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultUntranslatedPageListService>().As<IUntranslatedPageListService>().InstancePerLifetimeScope();

            // Registering root module, because root module register the last one, and this one should be before users module
            containerBuilder.RegisterType<EmptyUserProfileUrlResolver>().As<IUserProfileUrlResolver>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>
        /// <returns>Enumerator of known module style sheet files.</returns>
        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[]
                {
                    new CssIncludeDescriptor(this, "bcms.pages.css")
                };
        }

        /// <summary>
        /// Gets known client side modules in page module.
        /// </summary>
        /// <returns>List of known client side modules in page module.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new JsIncludeDescriptor[]
                {
                    pagesJsModuleIncludeDescriptor,
                    pagePropertiesJsModuleIncludeDescriptor,
                    pagesContentJsModuleIncludeDescriptor,
                    redirectsJsModuleIncludeDescriptor,
                    seoJsModuleIncludeDescriptor,
                    widgetsJsModuleIncludeDescriptor,
                    templatesJsModuleIncludeDescriptor,
                    masterPagesJsModuleIncludeDescriptor,
                    historyJsModuleIncludeDescriptor,
                    sitemapJsModuleIncludeDescriptor,
                    new PagesLanguagesJsModuleIncludeDescriptor(this), 
                    new JsIncludeDescriptor(this, "bcms.pages.filter")
                };
        }

        /// <summary>
        /// Registers the sidebar main projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns>Sidebar main action projections.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSidebarMainProjections(ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
                {
                    new InheriteProjection(
                        "div",
                        new IPageActionProjection[]
                            {
                                new LinkToNewTabProjection
                                {
                                    InnerText = page => RootGlobalization.Authentication_ViewAsPublic_Public,
                                    LinkAddress = page => page.PageUrl,
                                    Order = 9,
                                    CssClass = page => "bcms-sidemenu-btn bcms-as-public"
                                },
                                new ButtonActionProjection(pagesJsModuleIncludeDescriptor, page => "changePublishStatus")
                                {
                                    Order = 10,
                                    Title = page => page.Status == PageStatus.Published ? PagesGlobalization.Sidebar_PageStatusUnpublish : PagesGlobalization.Sidebar_PageStatusPublish,
                                    CssClass = page => page.Status == PageStatus.Published ? "bcms-sidemenu-btn bcms-btn-ok" : "bcms-sidemenu-btn bcms-btn-warn",
                                    AccessRole = RootModuleConstants.UserRoles.PublishContent,
                                    ShouldBeRendered = page => !page.IsMasterPage && !IsReadOnly(page)
                                }
                             })
                        {
                            Order = 10,
                            CssClass = page => "bcms-buttons-block"
                        }, 
                    
                     new ButtonActionProjection(seoJsModuleIncludeDescriptor, page => "openEditSeoDialog")
                            {
                                Order = 20,
                                Title = page => PagesGlobalization.Sidebar_EditSeoButtonTitle,
                                CssClass = page => page.HasSEO ? "bcms-sidemenu-btn bcms-btn-ok" : "bcms-sidemenu-btn bcms-btn-warn",
                                AccessRole = RootModuleConstants.UserRoles.EditContent,
                                ShouldBeRendered = page => !page.IsMasterPage
                            },

                    new EditPagePropertiesButtonProjection(pagePropertiesJsModuleIncludeDescriptor, page => page.IsMasterPage ? "editMasterPageProperties" : "editPageProperties")
                            {
                                Order = 30,
                                Title = page => page.IsMasterPage 
                                    ? PagesGlobalization.Sidebar_EditMasterPagePropertiesButtonTitle
                                    : PagesGlobalization.Sidebar_EditPagePropertiesButtonTitle,
                                CssClass = page => "bcms-sidemenu-btn"
                            },

                    new SeparatorProjection(40) { CssClass = page => "bcms-sidebar-separator" }, 

                    new InheriteProjection(
                        "div",
                        new IPageActionProjection[]
                            {
                                new ButtonActionProjection(pagesJsModuleIncludeDescriptor, page => "addNewPage")
                                {
                                    Order = 10,
                                    Title = page => PagesGlobalization.Sidebar_AddNewPageButtonTitle,
                                    CssClass = page => "bcms-sidemenu-btn bcms-btn-add",
                                    AccessRole = RootModuleConstants.UserRoles.EditContent
                                },
                                new ButtonActionProjection(pagesJsModuleIncludeDescriptor, page => "clonePage")
                                {
                                    Order = 30,
                                    Title = page => PagesGlobalization.Siderbar_ClonePageButtonTitle,
                                    CssClass = page => "bcms-sidemenu-btn bcms-btn-clone",
                                    AccessRole = RootModuleConstants.UserRoles.EditContent
                                }
                             })
                        {
                            Order = 50,
                            CssClass = page => "bcms-buttons-block"
                        }, 
                        
                    new ButtonActionProjection(masterPagesJsModuleIncludeDescriptor, page => "addMasterPage")
                        {
                            Title = page => PagesGlobalization.Sidebar_CreateMasterPageButtonTitle,
                            CssClass = page => "bcms-sidemenu-btn bcms-btn-add js-redirect-to-new-page",
                            Order = 300,
                            ShouldBeRendered = page => page.IsMasterPage,
                            Id = page => "bcms-create-page-button-side-panel",
                            AccessRole = RootModuleConstants.UserRoles.EditContent
                        },
                        
                    new ButtonActionProjection(pagesJsModuleIncludeDescriptor, page => "translatePage")
                        {
                            Title = page => PagesGlobalization.Sidebar_TranslatePageButtonTitle,
                            CssClass = page => "bcms-sidemenu-btn bcms-btn-translate",
                            Order = 400,
                            ShouldBeRendered = page => CmsContext.Config.EnableMultilanguage && !page.IsMasterPage,
                            AccessRole = RootModuleConstants.UserRoles.EditContent
                        },

                    new ButtonActionProjection(pagesJsModuleIncludeDescriptor, page => "deleteCurrentPage")
                        {
                            Order = 800,
                            Title = page => PagesGlobalization.Sidebar_DeletePageButtonTitle,
                            CssClass = page => "bcms-sidemenu-btn bcms-btn-delete",
                            AccessRole = RootModuleConstants.UserRoles.DeleteContent
                        }
                };
        }

        private bool ShouldBeRendered(IPage page)
        {
            return (page is PageProperties) ? !page.IsMasterPage && !((PageProperties)page).IsReadOnly : !page.IsMasterPage;
        }

        /// <summary>
        /// Registers the sidebar side projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns>Sidebar action projections.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSidebarSideProjections(ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
                {
                      new HtmlElementProjection("div")
                          {
                              Id = page => "bcms-sidemenu-pubstatus",
                              Tooltip = page => page.Status == PageStatus.Published 
                                                    ? PagesGlobalization.Sidebar_PageStatusPublishedTooltip
                                                    : PagesGlobalization.Sidebar_PageStatusUnpublishedTooltip,
                              Order = 10,
                              CssClass = page => page.Status == PageStatus.Published 
                                                    ? "bcms-sidemenu-pubstatus"
                                                    : "bcms-sidemenu-pubstatus bcms-pubstatus-warn",
                              ShouldBeRendered = page => !page.IsMasterPage
                          }, 

                      new HtmlElementProjection("div")
                          {
                              Id = page => "bcms-sidemenu-seostatus",
                              Tooltip = page => page.HasSEO 
                                                    ? PagesGlobalization.Sidebar_PageStatusSeoOkTooltip
                                                    : PagesGlobalization.Sidebar_PageStatusNoSeoTooltip,
                              Order = 20,
                              CssClass = page => page.HasSEO 
                                                    ? "bcms-sidemenu-seostatus"
                                                    : "bcms-sidemenu-seostatus bcms-seostatus-warn",
                              ShouldBeRendered = page => !page.IsMasterPage
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
            return new IPageActionProjection[]
                {
                    new LinkActionProjection(pagesJsModuleIncludeDescriptor, page => "loadSiteSettingsPageList")
                        {
                            Order = 1000,
                            Title = page => PagesGlobalization.SiteSettings_PagesMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.DeleteContent)
                        },
                    
                    new SeparatorProjection(1500), 

                    new SeparatorProjection(2500), 

                    new LinkActionProjection(widgetsJsModuleIncludeDescriptor, page => "loadSiteSettingsWidgetList")
                        {
                            Order = 3000,
                            Title = page => PagesGlobalization.SiteSettings_WidgetsMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.Administration
                        },
                        
                     new LinkActionProjection(templatesJsModuleIncludeDescriptor, page => "loadSiteSettingsTemplateList")
                        {
                            Order = 3100,
                            Title = page => PagesGlobalization.SiteSettings_TemplatesMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.Administration
                        },

                    new SeparatorProjection(3500), 

                    new LinkActionProjection(redirectsJsModuleIncludeDescriptor, page => "loadSiteSettingsRedirectList")
                        {
                            Order = 4000,
                            Title = page => PagesGlobalization.SiteSettings_Redirects,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.EditContent
                        },

                    new LinkActionProjection(sitemapJsModuleIncludeDescriptor, page => "loadSiteSettingsSitemapList")
                        {
                            Order = 4500,
                            Title = page => NavigationGlobalization.SiteSettings_SitemapMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.EditContent
                        }                                      
                };
        }

        /// <summary>
        /// Occurs, when the page is retrieved.
        /// </summary>
        /// <param name="args">The <see cref="PageRetrievedEventArgs" /> instance containing the event data.</param>
        private void Events_PageRetrieved(PageRetrievedEventArgs args)
        {
            if (args != null && args.RenderPageData != null)
            {
                ExtendPageWithPageData(args.RenderPageData, args.PageData);
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

            renderPageViewModel.ExtendWithPageData(pageData);
        }

        private void RegisterRenderingPageProperties()
        {
            PageHtmlRenderer.Register(new RenderingPageMainImageUrlProperty());
            PageHtmlRenderer.Register(new RenderingPageSecondaryImageUrlProperty());
            PageHtmlRenderer.Register(new RenderingPageFeaturedImageUrlProperty());
            PageHtmlRenderer.Register(new RenderingPageCategoryProperty());
        }

        private bool IsReadOnly(IPage page)
        {
            return (page is RenderPageViewModel) && ((RenderPageViewModel)page).IsReadOnly;
        }
    }
}
