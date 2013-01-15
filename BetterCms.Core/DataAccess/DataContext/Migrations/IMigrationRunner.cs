using System.Collections.Generic;

using BetterCms.Core.Modules;

namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    /// <summary>
    /// Defines contract to run database migrations.
    /// </summary>
    public interface IMigrationRunner
    {
        /// <summary>
        /// Runs migrations from the specified assemblies.
        /// </summary>
        void Migrate(IList<ModuleDescriptor> moduleDescriptors, bool up = true);
    }
}
