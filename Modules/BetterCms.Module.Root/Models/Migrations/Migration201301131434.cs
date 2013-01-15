
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;
using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201301131434)]
    public class Migration201301131434 : DefaultMigration
    {
        public Migration201301131434()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter
                  .Table("Regions").InSchema(SchemaName)
                  .AddColumn("Description").AsAnsiString(MaxLength.Name).Nullable();

            Delete.Column("Name").FromTable("Regions").InSchema(SchemaName);
        }

        public override void Down()
        {
            
        }
    }
}