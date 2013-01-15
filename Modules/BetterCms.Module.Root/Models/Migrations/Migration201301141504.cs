
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;
using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201301141504)]
    public class Migration201301141504 : DefaultMigration
    {
        public Migration201301141504()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter.Table("LayoutRegions").InSchema(SchemaName).AddColumn("Description").AsAnsiString(MaxLength.Name).Nullable();
            Delete.Column("Description").FromTable("Regions").InSchema(SchemaName);
        }

        public override void Down()
        {
            Alter.Table("Regions").InSchema(SchemaName).AddColumn("Description").AsAnsiString(MaxLength.Name).Nullable();
            Delete.Column("Description").FromTable("LayoutRegions").InSchema(SchemaName);
        }
    }
}