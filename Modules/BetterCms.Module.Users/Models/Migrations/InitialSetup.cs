
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.Models.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(20130181125)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string usersModuleSchemaName;

        /// <summary>
        /// The media manager schema name.
        /// </summary>
        private readonly string UsersSchemaName;

        public InitialSetup()
            : base(UsersModuleDescriptor.ModuleName)
        {
            usersModuleSchemaName = (new UsersVersionTableMetaData()).SchemaName;
            UsersSchemaName = (new UsersVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            Create.Table("Users").InSchema(SchemaName).WithCmsBaseColumns()
                   .WithColumn("UserName").AsAnsiString(MaxLength.Name).NotNullable()
                   .WithColumn("FirstName").AsAnsiString(MaxLength.Name).Nullable()
                   .WithColumn("LastName").AsAnsiString(MaxLength.Name).Nullable()
                   .WithColumn("Email").AsAnsiString(MaxLength.Email).NotNullable()
                   .WithColumn("Password").AsAnsiString(MaxLength.Name).NotNullable()
                   .WithColumn("ImageId").AsGuid().Nullable();
        }

        public override void Down()
        {           
            Delete.Table("Users").InSchema(SchemaName);
        }
    }
}