using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Autofac;

using BetterCms.Core.Modules;
using BetterCms.Module.Viddler.Registration;
using BetterCms.Module.Viddler.Services;

namespace BetterCms.Module.Viddler
{
    /// <summary>
    /// Pages module descriptor.
    /// </summary>
    public class ViddlerModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "viddler";

        /// <summary>
        /// The viddler area name.
        /// </summary>
        internal const string ViddlerAreaName = "bcms-viddler";

        /// <summary>
        /// The viddler java script module descriptor.
        /// </summary>
        private readonly ViddlerJsModuleIncludeDescriptor viddlerJsModuleIncludeDescriptor;

        /// <summary>
        /// The videos java script module include descriptor.
        /// </summary>
        private readonly VideosJsModuleIncludeDescriptor videosJsModuleIncludeDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViddlerModuleDescriptor" /> class.
        /// </summary>
        public ViddlerModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
            videosJsModuleIncludeDescriptor = new VideosJsModuleIncludeDescriptor(this);
            viddlerJsModuleIncludeDescriptor = new ViddlerJsModuleIncludeDescriptor(this);
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
                return "A viddler module for Better CMS.";
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
                return ViddlerAreaName;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
#if DEBUG // TODO: REMOVE this when test account will be available.
            containerBuilder.RegisterType<MockupViddlerService>().AsImplementedInterfaces().InstancePerLifetimeScope();
#else
            containerBuilder.RegisterType<DefaultViddlerService>().AsImplementedInterfaces().InstancePerLifetimeScope();
#endif
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
                    viddlerJsModuleIncludeDescriptor
                };
        }
    }
}
