using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201301031618)]
    public class Migration201301031618 : DefaultMigration
    {
        public Migration201301031618()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter
                .Table("MediaImages").InSchema(SchemaName)                
                .AddColumn("PublicThumbnailUrl").AsString(MaxLength.Url).NotNullable().WithDefaultValue("");
        }

        public override void Down()
        {            
            Delete
               .Column("PublicThumbnailUrl")
               .FromTable("MediaImages").InSchema(SchemaName);
        }     
    }
}