using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201511051430)]
    public class Migration201511051430 : DefaultMigration
    {
        public Migration201511051430()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter.Table("ChildContentOptions").InSchema(SchemaName).AddColumn("UseDefaultValue").AsBoolean().NotNullable().WithDefaultValue(false);
            Alter.Table("PageContentOptions").InSchema(SchemaName).AddColumn("UseDefaultValue").AsBoolean().NotNullable().WithDefaultValue(false);
            Alter.Table("PageOptions").InSchema(SchemaName).AddColumn("UseDefaultValue").AsBoolean().NotNullable().WithDefaultValue(false);
        }
    }
}