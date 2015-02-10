using System.Collections.Generic;

using Devbridge.Platform.Core.Modules;

namespace Devbridge.Platform.Core.DataAccess.DataContext.Migrations
{
    /// <summary>
    /// Defines contract to run database migrations.
    /// </summary>
    public interface IMigrationRunner
    {
        /// <summary>
        /// Runs migrations from the specified modules.
        /// </summary>
        void MigrateStructure(IList<ModuleDescriptor> moduleDescriptors);
    }
}
