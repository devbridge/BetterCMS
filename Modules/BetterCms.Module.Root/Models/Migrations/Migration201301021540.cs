using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201301021540)]
    public class Migration201301021540 : DefaultMigration
    {
        public Migration201301021540()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create
                .Table("Settings").InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid);
        }

        public override void Down()
        {
            Delete.Table("Settings").InSchema(SchemaName);
        }     
    }
}