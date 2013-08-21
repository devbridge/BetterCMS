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
        private readonly string mediaModuleSchemaName = (new MediaManager.Models.Migrations.MediaManagerVersionTableMetaData()).SchemaName;

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
            CreateUserRolesTable();
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
                .WithColumn("Password").AsAnsiString(MaxLength.Password).NotNullable()
                .WithColumn("Salt").AsAnsiString(MaxLength.Password).NotNullable()
                .WithColumn("ImageId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_Users_ImageId_Cms_MediaFiles_Id")
                .FromTable("Users").InSchema(SchemaName)
                .ForeignColumn("ImageId")
                .ToTable("MediaFiles").InSchema(mediaModuleSchemaName)
                .PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the roles table.
        /// </summary>
        private void CreateRolesTable()
        {
            Create
                .Table("Roles").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsAnsiString(MaxLength.Name).NotNullable();
        }

        /// <summary>
        /// Creates the user roles table.
        /// </summary>
        private void CreateUserRolesTable()
        {
            Create.Table("UserRoles").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("RoleId").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable();

            Create.ForeignKey("FK_Cms_UserRoles_Role_Cms_Role_Id")
                .FromTable("UserRoles").InSchema(SchemaName)
                .ForeignColumn("RoleId")
                .ToTable("Roles").InSchema(SchemaName)
                .PrimaryColumn("Id");

            Create.ForeignKey("FK_Cms_UserRoles_User_Cms_Users_Id")
                .FromTable("UserRoles").InSchema(SchemaName)
                .ForeignColumn("UserId")
                .ToTable("Users").InSchema(SchemaName)
                .PrimaryColumn("Id");
        }
    }
}