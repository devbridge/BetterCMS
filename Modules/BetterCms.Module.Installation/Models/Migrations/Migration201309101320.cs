using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Installation.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201309101320)]
    public class Migration201309101320: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201309101320"/> class.
        /// </summary>
        public Migration201309101320()
            : base(InstallationModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            // Initial setup for the Installation module already executed and created dbo.VersionInfo table.
            if (Schema.Schema("dbo").Exists() && Schema.Schema("dbo").Table("VersionInfo").Exists())
            {
                Delete.Table("VersionInfo").InSchema("dbo");
            }
        }
    }
}