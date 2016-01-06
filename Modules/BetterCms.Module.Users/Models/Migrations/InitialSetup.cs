using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201308200000)]
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

            PopulateDefaultRoles();
        }

        /// <summary>
        /// Creates the users table.
        /// </summary>
        private void CreateUsersTable()
        {
            Create.Table("Users").InSchema(SchemaName).WithBaseColumns()
                .WithColumn("UserName").AsAnsiString(UsersModuleConstants.UserNameMaxLength).NotNullable()
                .WithColumn("FirstName").AsAnsiString(MaxLength.Name).NotNullable()
                .WithColumn("LastName").AsAnsiString(MaxLength.Name).NotNullable()
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

            Create
                .UniqueConstraint("UX_Cms_Users_UserName")
                .OnTable("Users").WithSchema(SchemaName)
                .Columns(new[] { "UserName", "DeletedOn" });

            Create
                .UniqueConstraint("UX_Cms_Users_Email")
                .OnTable("Users").WithSchema(SchemaName)
                .Columns(new[] { "Email", "DeletedOn" });
        }

        /// <summary>
        /// Creates the roles table.
        /// </summary>
        private void CreateRolesTable()
        {
            Create
                .Table("Roles").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("Name").AsAnsiString(MaxLength.Name).NotNullable()
                .WithColumn("DisplayName").AsAnsiString(MaxLength.Name).Nullable()
                .WithColumn("IsSystematic").AsBoolean().NotNullable().WithDefaultValue(false);

            Create
                .UniqueConstraint("UX_Cms_Roles_Name")
                .OnTable("Roles").WithSchema(SchemaName)
                .Columns(new[] { "Name", "DeletedOn" });
        }

        /// <summary>
        /// Creates the user roles table.
        /// </summary>
        private void CreateUserRolesTable()
        {
            Create.Table("UserRoles").InSchema(SchemaName)
                .WithBaseColumns()
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

        /// <summary>
        /// Populates the default roles.
        /// </summary>
        private void PopulateDefaultRoles()
        {
            Insert.IntoTable("Roles")
                  .InSchema(SchemaName)
                  .Row(new {
                              Version = 1,
                              IsDeleted = false,
                              CreatedOn = System.DateTime.Now,
                              CreatedByUser = "Better CMS",
                              ModifiedOn = System.DateTime.Now,
                              ModifiedByUser = "Better CMS",
                              Name = "BcmsEditContent",
                              DisplayName = "Better CMS: edit content",
                              IsSystematic = true
                          })
                  .Row(new {
                              Version = 1,
                              IsDeleted = false,
                              CreatedOn = System.DateTime.Now,
                              CreatedByUser = "Better CMS",
                              ModifiedOn = System.DateTime.Now,
                              ModifiedByUser = "Better CMS",
                              Name = "BcmsPublishContent",
                              DisplayName = "Better CMS: publish content",
                              IsSystematic = true
                          })
                  .Row(new {
                              Version = 1,
                              IsDeleted = false,
                              CreatedOn = System.DateTime.Now,
                              CreatedByUser = "Better CMS",
                              ModifiedOn = System.DateTime.Now,
                              ModifiedByUser = "Better CMS",
                              Name = "BcmsDeleteContent",
                              DisplayName = "Better CMS: delete content",
                              IsSystematic = true
                          })
                  .Row(new {
                              Version = 1,
                              IsDeleted = false,
                              CreatedOn = System.DateTime.Now,
                              CreatedByUser = "Better CMS",
                              ModifiedOn = System.DateTime.Now,
                              ModifiedByUser = "Better CMS",
                              Name = "BcmsAdministration",
                              DisplayName = "Better CMS: administrator",
                              IsSystematic = true
                          });
        }
    }
}