using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    [Migration(3)]
    public class InitialSetupCreatePageNotFound404Page : DefaultMigration
    {
        private const string RootModuleSchemaName = "bcms_root";

        public InitialSetupCreatePageNotFound404Page()
            : base(PagesModuleDescriptor.PagesModuleName)
        {
        }

        public override void Up()
        {
            Guid id = Guid.NewGuid();
            Insert.IntoTable("Pages").InSchema(RootModuleSchemaName).Row(new
                                                                             {
                                                                                 Id = id,
                                                                                 Version = 1,
                                                                                 CreatedOn = DateTime.Now,
                                                                                 CreatedByUser = "BetterCMS",
                                                                                 ModifiedOn = DateTime.Now,
                                                                                 ModifiedByUser = "BetterCMS",
                                                                                 PageUrl = "/404/",
                                                                                 Title = "Page Not Found",
                                                                                 LayoutId = 
                                                                             });

        }

        public override void Down()
        {
           
        }        
    }
}