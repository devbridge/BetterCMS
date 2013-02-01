
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.Models.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(20130181125)]
    public class InitialSetup : DefaultMigration
    {
        public InitialSetup()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {          
            CreateUsersTable();
            CreateRolesTable();
            CreatePermissionsTable();
            CreateRolePremissions();
        }

        public override void Down()
        {
            RemoveRolePremissions();
            RemoveRolesTable();
            RemovePersmissionsTable();
            RemoveUsersTable();
        }

        private void CreateUsersTable()
        {
            Create.Table("Users").InSchema(SchemaName).WithCmsBaseColumns()
                 .WithColumn("UserName").AsAnsiString(MaxLength.Name).NotNullable()
                 .WithColumn("FirstName").AsAnsiString(MaxLength.Name).Nullable()
                 .WithColumn("LastName").AsAnsiString(MaxLength.Name).Nullable()
                 .WithColumn("Email").AsAnsiString(MaxLength.Email).NotNullable()
                 .WithColumn("Password").AsAnsiString(MaxLength.Name).NotNullable()
                 .WithColumn("ImageId").AsGuid().Nullable();
        }

        private void RemoveUsersTable()
        {
            Delete.Table("Users").InSchema(SchemaName);
        }

        private void CreateRolesTable()
        {
            Create.Table("Roles").InSchema(SchemaName).WithCmsBaseColumns().WithColumn("Name").AsAnsiString(MaxLength.Name).NotNullable();
        }

        private void RemoveRolesTable()
        {
            Delete.Table("Roles").InSchema(SchemaName);
        }

        private void CreatePermissionsTable()
        {
            Create
                .Table("Permissions").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsAnsiString(MaxLength.Name).NotNullable().Unique()
                .WithColumn("Description").AsAnsiString(MaxLength.Name).NotNullable();            
        }

        private void RemovePersmissionsTable()
        {
            Delete.Table("Permissions").InSchema(SchemaName);
        }

        private void CreateRolePremissions()
        {
            Create.Table("RolePermissions").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("RoleId").AsGuid().NotNullable()
                .WithColumn("PermissionId").AsGuid().NotNullable();

            Create
                .UniqueConstraint("UX_Cms_RolePermissions_RoleId_PermissionId")
                .OnTable("RolePermissions").WithSchema(SchemaName)
                .Columns(new[] { "RoleId", "PermissionId", "DeletedOn" });

            Create
                .ForeignKey("FK_Cms_RolePermissions_Cms_Roles")
                .FromTable("RolePermissions").InSchema(SchemaName).ForeignColumn("RoleId")
                .ToTable("Roles").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_RolePermissions_Cms_Permissions")
                .FromTable("RolePermissions").InSchema(SchemaName).ForeignColumn("PermissionId")
                .ToTable("Permissions").InSchema(SchemaName).PrimaryColumn("Id");
        }

        private void RemoveRolePremissions()
        {
            Delete.UniqueConstraint("UX_Cms_RolePermissions_RoleId_PermissionId").FromTable("RolePermissions").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_RolePermissions_Cms_Roles").OnTable("RolePermissions").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_RolePermissions_Cms_Permissions").OnTable("RolePermissions").InSchema(SchemaName);
            Delete.Table("RolePermissions").InSchema(SchemaName);
        }
    }
}