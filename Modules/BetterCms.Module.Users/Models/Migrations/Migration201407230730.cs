using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201407230730)]
    public class Migration201407230730: DefaultMigration
    {
        public Migration201407230730()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }
        
        public override void Up()
        {
            var update = new { Description = "Can publish Better CMS pages and page contents.", ModifiedOn = DateTime.Now, ModifiedByUser = "Admin" };
            var where = new { Description = "Can publish Beter CMS pages and page contents.", Name = "BcmsPublishContent", IsSystematic = 1, IsDeleted = 0 }; 

            Update.Table("Roles").InSchema(SchemaName).Set(update).Where(where);
        }
    }
}