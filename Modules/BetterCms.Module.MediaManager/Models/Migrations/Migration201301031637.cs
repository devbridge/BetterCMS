using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201301031637)]
    public class Migration201301031637 : DefaultMigration
    {
        public Migration201301031637()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter
                .Table("MediaImages").InSchema(SchemaName)                
                .AddColumn("PublicOriginallUrl").AsString(MaxLength.Url).NotNullable().WithDefaultValue("");
        }

        public override void Down()
        {            
            Delete
               .Column("PublicOriginallUrl")
               .FromTable("MediaImages").InSchema(SchemaName);
        }     
    }
}