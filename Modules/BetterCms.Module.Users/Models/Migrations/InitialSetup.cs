using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(20130181125)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {          
            CreateUsersTable();
            CreateRolesTable();
            CreatePermissionsTable();
            CreateRolePermissions();
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the users table.
        /// </summary>
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

        /// <summary>
        /// Creates the roles table.
        /// </summary>
        private void CreateRolesTable()
        {
            Create.Table("Roles").InSchema(SchemaName).WithCmsBaseColumns().WithColumn("Name").AsAnsiString(MaxLength.Name).NotNullable();
        }

        /// <summary>
        /// Creates the permissions table.
        /// </summary>
        private void CreatePermissionsTable()
        {
            Create
                .Table("Permissions").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsAnsiString(MaxLength.Name).NotNullable().Unique()
                .WithColumn("Description").AsAnsiString(MaxLength.Name).NotNullable();            
        }

        /// <summary>
        /// Creates the role permissions.
        /// </summary>
        private void CreateRolePermissions()
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
    }
}