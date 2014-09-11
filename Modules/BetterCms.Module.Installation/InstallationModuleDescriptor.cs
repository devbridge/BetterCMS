using System;
using System.Collections.Generic;

using BetterCms.Core.Modules;

namespace BetterCms.Module.Installation
{
    /// <summary>
    /// Templates module descriptor.
    /// </summary>
    public class InstallationModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "installation";
        
        internal const string ModuleSchemaName = "bcms_installation";

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationModuleDescriptor" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public InstallationModuleDescriptor(ICmsConfiguration configuration) : base(configuration)
        {            
        }

        internal const string ModuleId = "042e857e-8f35-44db-b512-b6584af05e05";

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
        /// Gets the name of the module database schema name.
        /// </summary>
        /// <value>
        /// The name of the module database schema.
        /// </value>
        public override string SchemaName
        {
            get
            {
                return ModuleSchemaName;
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
                return "Templates module for Better CMS.";
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
        /// Registers the style sheet files.
        /// </summary>
        /// <returns>
        /// Enumerator of known module style sheet files.
        /// </returns>
        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[] { new CssIncludeDescriptor(this, "bcms.installation.css", "bcms.installation.min.css", true) };
        }
    }
}
