using BetterCms.Core.Modules;

namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    /// <summary>
    /// Defines contract to run database migrations.
    /// </summary>
    public interface IMigrationRunner
    {
        /// <summary>
        /// Runs database migrations of the specified module descriptor.
        /// </summary>
        /// <param name="moduleDescriptor">The module descriptor.</param>
        /// <param name="up">if set to <c>true</c> migrates up; otherwise migrates down.</param>
        void Migrate(ModuleDescriptor moduleDescriptor, bool up = true);
    }
}
