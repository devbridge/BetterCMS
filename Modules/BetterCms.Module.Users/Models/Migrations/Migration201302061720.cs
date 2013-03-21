using BetterCms.Core.DataAccess.DataContext.Migrations;
using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201302061720)]
    public class Migration201302061720 : DefaultMigration
    {
        public Migration201302061720()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create.Table("UserRoles").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("RoleId").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable();

            Create.ForeignKey("FK_Cms_UserRoles_Role_Cms_Role_Id")
                  .FromTable("UserRoles")
                  .InSchema(SchemaName)
                  .ForeignColumn("RoleId")
                  .ToTable("Roles")
                  .InSchema(SchemaName)
                  .PrimaryColumn("Id");

            Create.ForeignKey("FK_Cms_UserRoles_User_Cms_Users_Id")
                  .FromTable("UserRoles")
                  .InSchema(SchemaName)
                  .ForeignColumn("UserId")
                  .ToTable("Users")
                  .InSchema(SchemaName)
                  .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Cms_UserRoles_Role_Cms_Role_Id").OnTable("UserRoles").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_UserRoles_User_Cms_Users_Id").OnTable("UserRoles").InSchema(SchemaName);
            Delete.Table("UserRoles").InSchema(SchemaName);            
        }
    }
}