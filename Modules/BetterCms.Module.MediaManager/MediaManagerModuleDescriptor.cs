using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Autofac;

using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Provider;
using BetterCms.Module.MediaManager.Registration;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Providers;

namespace BetterCms.Module.MediaManager
{
    /// <summary>
    /// Pages module descriptor.
    /// </summary>
    public class MediaManagerModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "media";

        internal const string MediaManagerAreaName = "bcms-media";

        /// <summary>
        /// The media java script module descriptor.
        /// </summary>
        private readonly MediaManagerJsModuleIncludeDescriptor mediaJsModuleIncludeDescriptor;

        /// <summary>
        /// The media history java script module descriptor.
        /// </summary>
        private readonly MediaHistoryJsModuleIncludeDescriptor mediaHistoryJsModuleIncludeDescriptor;

        /// <summary>
        /// The media upload module descriptor.
        /// </summary>
        private readonly MediaUploadJsModuleIncludeDescriptor mediaUploadModuleIncludeDescriptor;

        /// <summary>
        /// The image editor module descriptor.
        /// </summary>
        private readonly ImageEditorJsModuleIncludeDescriptor imageEditorModuleIncludeDescriptor;

        /// <summary>
        /// The file editor module include descriptor.
        /// </summary>
        private readonly FileEditorJsModuleIncludeDescriptor fileEditorModuleIncludeDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaManagerModuleDescriptor" /> class.
        /// </summary>
        public MediaManagerModuleDescriptor(ICmsConfiguration cmsConfiguration) : base(cmsConfiguration)
        {
            mediaJsModuleIncludeDescriptor = new MediaManagerJsModuleIncludeDescriptor(this);
            mediaUploadModuleIncludeDescriptor = new MediaUploadJsModuleIncludeDescriptor(this);
            imageEditorModuleIncludeDescriptor = new ImageEditorJsModuleIncludeDescriptor(this);
            fileEditorModuleIncludeDescriptor = new FileEditorJsModuleIncludeDescriptor(this);
            mediaHistoryJsModuleIncludeDescriptor = new MediaHistoryJsModuleIncludeDescriptor(this);

            // Register images gallery custom option: album
            CustomOptionsProvider.RegisterProvider(MediaManagerFolderOptionProvider.Identifier, new MediaManagerFolderOptionProvider());
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
                return "A media manager module for Better CMS.";
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
                return MediaManagerAreaName;
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
                return int.MaxValue - 200;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {

            containerBuilder.RegisterType<DefaultMediaFileUrlResolver>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultMediaFileService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultMediaImageService>().AsImplementedInterfaces().InstancePerLifetimeScope();            
            containerBuilder.RegisterType<DefaultMediaHistoryService>().AsImplementedInterfaces().InstancePerLifetimeScope();            
            containerBuilder.RegisterType<DefaultTagService>().AsImplementedInterfaces().InstancePerLifetimeScope();            
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>        
        /// <returns>Enumerator of known module style sheet files.</returns>
        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[]
                {
                    new CssIncludeDescriptor(this, "bcms.media.css")
                };
        }

        /// <summary>
        /// Gets known client side modules in page module.
        /// </summary>        
        /// <returns>List of known client side modules in page module.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {            
            return new[]
                {
                    mediaJsModuleIncludeDescriptor,
                    mediaUploadModuleIncludeDescriptor,
                    imageEditorModuleIncludeDescriptor,
                    fileEditorModuleIncludeDescriptor,
                    mediaHistoryJsModuleIncludeDescriptor,
                    new JsIncludeDescriptor(this, "bcms.html5Upload"),
                    new JsIncludeDescriptor(this, "bcms.jquery.jcrop"),
                    new JsIncludeDescriptor(this, "bcms.contextMenu")
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
                    new LinkActionProjection(mediaJsModuleIncludeDescriptor, page => "loadSiteSettingsMediaManager")
                        {
                            Order = 2400,
                            Title = page => MediaGlobalization.SiteSettings_MediaManagerMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.Administration, RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.DeleteContent)
                        }                                      
                };
        }
    }
}
