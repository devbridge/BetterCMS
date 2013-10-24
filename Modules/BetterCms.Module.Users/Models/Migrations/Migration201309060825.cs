using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201309060825)]
    public class Migration201309060825 : DefaultMigration
    {
        public Migration201309060825()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }
        
        public override void Up()
        {
            // Users table's columns FirstName and LastName are not mandatory anymore.
            Alter.Table("Users").InSchema(SchemaName).AlterColumn("FirstName").AsString(MaxLength.Name).Nullable();
            Alter.Table("Users").InSchema(SchemaName).AlterColumn("LastName").AsString(MaxLength.Name).Nullable();

            // Drop unique constraints
            if (Schema.Schema(SchemaName).Table("Users").Index("UX_Cms_Users_UserName").Exists())
            {
                Delete.UniqueConstraint("UX_Cms_Users_UserName").FromTable("Users").InSchema(SchemaName);
            }
            if (Schema.Schema(SchemaName).Table("Users").Index("UX_Cms_Users_Email").Exists())
            {
                Delete.UniqueConstraint("UX_Cms_Users_Email").FromTable("Users").InSchema(SchemaName);
            }

            // Other columns changed to nvarchchar
            Alter.Table("Users").InSchema(SchemaName).AlterColumn("UserName").AsString(UsersModuleConstants.UserNameMaxLength).NotNullable();
            Alter.Table("Users").InSchema(SchemaName).AlterColumn("Email").AsString(MaxLength.Email).NotNullable();

            // Re-create unique constraints
            Create
                .UniqueConstraint("UX_Cms_Users_UserName")
                .OnTable("Users").WithSchema(SchemaName)
                .Columns(new[] { "UserName", "DeletedOn" });

            Create
                .UniqueConstraint("UX_Cms_Users_Email")
                .OnTable("Users").WithSchema(SchemaName)
                .Columns(new[] { "Email", "DeletedOn" });
        }
    }
}