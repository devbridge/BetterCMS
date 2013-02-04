using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201302011135)]
    public class Migration201302011135 : DefaultMigration
    {
        private readonly string mediaModuleSchemaName;
        public Migration201302011135()
            : base(UsersModuleDescriptor.ModuleName)
        {
            mediaModuleSchemaName = (new MediaManager.Models.Migrations.MediaManagerVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            Create
            .ForeignKey("FK_Cms_Users_ImageId_Cms_MediaFiles_Id")
            .FromTable("Users").InSchema(SchemaName).ForeignColumn("ImageId")
            .ToTable("MediaFiles").InSchema(mediaModuleSchemaName).PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Cms_Users_Cms_MediaFiles").OnTable("Users").InSchema(SchemaName);
        }
    }
}