using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201302211715)]
    public class Migration201302211715 : DefaultMigration
    {
        public Migration201302211715()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter.Table("Users").InSchema(SchemaName).AlterColumn("Password").AsAnsiString(MaxLength.Password).NotNullable();
            Alter.Table("Users").InSchema(SchemaName).AddColumn("Salt").AsAnsiString(MaxLength.Password).NotNullable();
        }

        public override void Down()
        {
            Alter.Table("Users").InSchema(SchemaName).AlterColumn("Password").AsAnsiString(MaxLength.Name);
            Delete.Column("Salt").FromTable("Users").InSchema(SchemaName);
        }
    }
}