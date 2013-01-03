using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(2013010211106)]
    public class Migration201301021106 : DefaultMigration
    {
        public Migration201301021106()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter
                 .Table("Layouts").InSchema(SchemaName)
                 .AddColumn("PreviewUrl").AsAnsiString(MaxLength.Url).Nullable();
        }

        public override void Down()
        {
          
        }     
    }
}