
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(20130181130)]
    public class Migration201301181130 : DefaultMigration
    {
        public Migration201301181130()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            /*Create.Table("Users").InSchema(SchemaName).WithCmsBaseColumns()
                .WithColumn("UserName").AsAnsiString(MaxLength.Name).NotNullable()
                .WithColumn("FirstName").AsAnsiString(MaxLength.Name).Nullable()
                .WithColumn("LastName").AsAnsiString(MaxLength.Name).Nullable()
                .WithColumn("Email").AsAnsiString(MaxLength.Email).NotNullable()
                .WithColumn("Password").AsAnsiString(MaxLength.Name).NotNullable()
                .WithColumn("ImageId").AsGuid().Nullable();
            
            Create
                .ForeignKey("FK_Cms_Users_ImageId_Medias_Id")
                .FromTable("Users").InSchema(SchemaName).ForeignColumn("ImageId")
                .ToTable("Medias").InSchema(SchemaName).PrimaryColumn("Id");*/
        }

        public override void Down()
        {
            /*Delete.ForeignKey("ImageId").OnTable("Users").InSchema(SchemaName);
            Delete.Table("Users").InSchema(SchemaName);*/
        }
    }
}