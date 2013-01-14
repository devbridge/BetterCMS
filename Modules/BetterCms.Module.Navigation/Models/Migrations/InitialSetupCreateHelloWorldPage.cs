using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    [Migration(2)]
    public class InitialSetupCreateHelloWorldPage : DefaultMigration
    {
        public InitialSetupCreateHelloWorldPage()
            : base(PagesModuleDescriptor.PagesModuleName)
        {
        }

        public override void Up()
        {
            throw new System.NotImplementedException();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}