using System.Collections.Generic;

using BetterCms.Core.Modules;

namespace BetterCms.Module.Templates
{
    /// <summary>
    /// Templates module descriptor.
    /// </summary>
    public class TemplatesModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "templates";

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplatesModuleDescriptor" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public TemplatesModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
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
                return "Templates module for BetterCMS.";
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

        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[] { new CssIncludeDescriptor(this, "bcms.templates.css") };
        }
    }
}
