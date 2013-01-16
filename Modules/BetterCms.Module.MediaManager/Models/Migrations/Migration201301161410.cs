using BetterCms.Core.DataAccess.DataContext.Migrations;
using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201301161410)]
    public class Migration201301161410 : DefaultMigration
    {
        public Migration201301161410()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {            
            Rename.Column("FileName").OnTable("MediaFiles").InSchema(SchemaName).To("OriginalFileName");
            Rename.Column("FileExtension").OnTable("MediaFiles").InSchema(SchemaName).To("OriginalFileExtension");
        }

        public override void Down()
        {
            Rename.Column("OriginalFileName").OnTable("MediaFiles").InSchema(SchemaName).To("FileName");
            Rename.Column("OriginalFileExtension").OnTable("MediaFiles").InSchema(SchemaName).To("FileExtension");
        }
    }
}