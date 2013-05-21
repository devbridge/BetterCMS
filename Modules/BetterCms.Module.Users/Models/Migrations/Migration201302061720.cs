using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201302061720)]
    public class Migration201302061720 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201302061720"/> class.
        /// </summary>
        public Migration201302061720()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
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

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}