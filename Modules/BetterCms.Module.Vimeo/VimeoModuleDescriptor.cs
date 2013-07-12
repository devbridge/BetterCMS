using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Autofac;

using BetterCms.Core.Modules;
using BetterCms.Module.Vimeo.Registration;
using BetterCms.Module.Vimeo.Services;

namespace BetterCms.Module.Vimeo
{
    /// <summary>
    /// Pages module descriptor.
    /// </summary>
    public class VimeoModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "vimeo";

        /// <summary>
        /// The vimeo area name.
        /// </summary>
        internal const string VimeoAreaName = "bcms-vimeo";

        /// <summary>
        /// The vimeo java script module descriptor.
        /// </summary>
        private readonly VimeoJsModuleIncludeDescriptor vimeoJsModuleIncludeDescriptor;

        /// <summary>
        /// The videos java script module include descriptor.
        /// </summary>
        private readonly VideosJsModuleIncludeDescriptor videosJsModuleIncludeDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="VimeoModuleDescriptor" /> class.
        /// </summary>
        public VimeoModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
            videosJsModuleIncludeDescriptor = new VideosJsModuleIncludeDescriptor(this);
            vimeoJsModuleIncludeDescriptor = new VimeoJsModuleIncludeDescriptor(this);
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
                return "A vimeo module for Better CMS.";
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
                return VimeoAreaName;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<VimeoOAuthService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultVimeoService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<VideoProviderForCmsService>().AsImplementedInterfaces().InstancePerLifetimeScope();
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
                    videosJsModuleIncludeDescriptor,
                    vimeoJsModuleIncludeDescriptor
                };
        }
    }
}
