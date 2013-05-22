using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Installation.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201301151849)]
    public class InitialSetup : DefaultMigration
    {
        private const string LayoutsTableName = "Layouts";
        private readonly string rootSchemaName = "bcms_root";

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup() : base(InstallationModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            // For backward compatibility.
            Update.Table(LayoutsTableName)
                  .InSchema(rootSchemaName)
                  .Set(new { LayoutPath = "~/Areas/bcms-installation/Views/Shared/WideLayout.cshtml" })
                  .Where(new { LayoutPath = "~/Areas/bcms-templates/Views/Shared/WideLayout.cshtml" });

            Update.Table(LayoutsTableName)
                  .InSchema(rootSchemaName)
                  .Set(new { LayoutPath = "~/Areas/bcms-installation/Views/Shared/TwoColumnsLayout.cshtml" })
                  .Where(new { LayoutPath = "~/Areas/bcms-templates/Views/Shared/TwoColumnsLayout.cshtml" });

            Update.Table(LayoutsTableName)
                  .InSchema(rootSchemaName)
                  .Set(new { LayoutPath = "~/Areas/bcms-installation/Views/Shared/ThreeColumnsLayout.cshtml" })
                  .Where(new { LayoutPath = "~/Areas/bcms-templates/Views/Shared/ThreeColumnsLayout.cshtml" });
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}