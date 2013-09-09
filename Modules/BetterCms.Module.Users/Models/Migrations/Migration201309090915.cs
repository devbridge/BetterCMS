using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201309090915)]
    public class Migration201309090915 : DefaultMigration
    {
        public Migration201309090915()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }
        
        public override void Up()
        {
            // Rename roles
            var w1 = new { Name = "BcmsEditContent", DisplayName = "Better CMS: edit content", IsSystematic = 1, IsDeleted = 0 };
            var u1 = new { DisplayName = "Better CMS: content editor", ModifiedOn = DateTime.Now, ModifiedByUser = "Admin" };

            var w2 = new { Name = "BcmsPublishContent", DisplayName = "Better CMS: publish content", IsSystematic = 1, IsDeleted = 0 };
            var u2 = new { DisplayName = "Better CMS: content publisher", ModifiedOn = DateTime.Now, ModifiedByUser = "Admin" };

            var w3 = new { Name = "BcmsDeleteContent", DisplayName = "Better CMS: delete content", IsSystematic = 1, IsDeleted = 0 };
            var u3 = new { DisplayName = "Better CMS: content remover", ModifiedOn = DateTime.Now, ModifiedByUser = "Admin" };

            var w4 = new { Name = "BcmsAdministration", DisplayName = "Better CMS: administrator", IsSystematic = 1, IsDeleted = 0 };
            var u4 = new { DisplayName = "Better CMS: settings", ModifiedOn = DateTime.Now, ModifiedByUser = "Admin" };

            Update.Table("Roles").InSchema(SchemaName).Set(u1).Where(w1);
            Update.Table("Roles").InSchema(SchemaName).Set(u2).Where(w2);
            Update.Table("Roles").InSchema(SchemaName).Set(u3).Where(w3);
            Update.Table("Roles").InSchema(SchemaName).Set(u4).Where(w4);
        }
    }
}