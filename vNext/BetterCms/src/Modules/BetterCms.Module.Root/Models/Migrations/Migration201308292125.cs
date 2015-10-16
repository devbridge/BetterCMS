using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201308292125)]
    public class Migration201308292125: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308292125"/> class.
        /// </summary>
        public Migration201308292125()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            if (!Schema.Schema(SchemaName).Table("AccessRules").Column("IsForRole").Exists())
            {
                Alter.Table("AccessRules").InSchema(SchemaName).AddColumn("IsForRole").AsBoolean().NotNullable().WithDefaultValue(true);
            }
        }       
    }
}