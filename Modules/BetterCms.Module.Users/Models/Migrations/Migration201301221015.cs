using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201301221015)]
    public class Migration201301221015 : DefaultMigration
    {
        public Migration201301221015()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create.Table("Roles").InSchema(SchemaName).WithCmsBaseColumns().WithColumn("Name").AsAnsiString(MaxLength.Name).NotNullable();
            Create.Table("Permissions").InSchema(SchemaName).WithCmsBaseColumns().WithColumn("Name").AsAnsiString(MaxLength.Name).NotNullable();
            CreateRolePremissions();
            InsertPremission(new Guid("B638896E-B8BB-472F-8DFF-A0B83FF1F36F"), "Administrator");
            InsertPremission(new Guid("A638896E-B8BB-472F-8DFF-A0B83FF1F36F"), "Owner");
            InsertPremission(new Guid("D634896E-B8BB-472F-8DFF-A0B83FF1F36F"), "Content Editor");
        }

        public override void Down()
        {
            DeleteRolePremissions();
            Delete.Table("Roles").InSchema(SchemaName);
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

        private void DeleteRolePremissions()
        {
            Delete.UniqueConstraint("UX_Cms_RolePermissions_RoleId_PermissionId").FromTable("RolePermissions").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_RolePermissions_Cms_Roles").OnTable("RolePermissions").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_RolePermissions_Cms_Permissions").OnTable("RolePermissions").InSchema(SchemaName);
            Delete.Table("RolePermissions").InSchema(SchemaName);
        }

         private void InsertPremission(Guid premissionId, string name)
         {
             Insert
               .IntoTable("Permissions").InSchema(SchemaName)
               .Row(new
               {
                   Id = premissionId,
                   Version = 1,
                   IsDeleted = false,
                   CreatedOn = DateTime.Now,
                   CreatedByUser = "Admin",
                   ModifiedOn = DateTime.Now,
                   ModifiedByUser = "Admin",
                   Name = name
               });
         }
    }
}