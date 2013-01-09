using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201212271220)]
    public class Migration201212271220 : DefaultMigration
    {
        public Migration201212271220()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter
                 .Table("Contents").InSchema(SchemaName)
                 .AddColumn("PreviewUrl").AsAnsiString(MaxLength.Url).Nullable();
        }

        public override void Down()
        {
            Delete.Column("PreviewUrl").FromTable("Contents").InSchema(SchemaName);
        }     
    }
}