using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201211121710)]
    public class Migration201211121710 : DefaultMigration
    {
        public Migration201211121710()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
        }

        public override void Down()
        {
          
        }     
    }
}