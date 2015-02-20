using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201309101400)]
    public class Migration201309101400: DefaultMigration
    {
        public Migration201309101400()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }
        
        public override void Up()
        {
            // Rename column
            Rename
                .Column("DisplayName")
                .OnTable("Roles").InSchema(SchemaName)
                .To("Description");

            // Rename descriptions (renames old versions and new versions of role dsiplay names)
            var w11 = new { Name = "BcmsEditContent", Description = "Better CMS: content editor", IsSystematic = 1, IsDeleted = 0 };
            var w12 = new { Name = "BcmsEditContent", Description = "Better CMS: edit content", IsSystematic = 1, IsDeleted = 0 };
            var u1 = new { Description = "Can create and edit Better CMS pages and page contents.", ModifiedOn = DateTime.Now, ModifiedByUser = "Admin" };

            var w21 = new { Name = "BcmsPublishContent", Description = "Better CMS: content publisher", IsSystematic = 1, IsDeleted = 0 };
            var w22 = new { Name = "BcmsPublishContent", Description = "Better CMS: publish content", IsSystematic = 1, IsDeleted = 0 };
            var u2 = new { Description = "Can publish Beter CMS pages and page contents.", ModifiedOn = DateTime.Now, ModifiedByUser = "Admin" };

            var w31 = new { Name = "BcmsDeleteContent", Description = "Better CMS: content remover", IsSystematic = 1, IsDeleted = 0 };
            var w32 = new { Name = "BcmsDeleteContent", Description = "Better CMS: delete content", IsSystematic = 1, IsDeleted = 0 };
            var u3 = new { Description = "Can delete Better CMS resources.", ModifiedOn = DateTime.Now, ModifiedByUser = "Admin" };

            var w41 = new { Name = "BcmsAdministration", Description = "Better CMS: settings", IsSystematic = 1, IsDeleted = 0 };
            var w42 = new { Name = "BcmsAdministration", Description = "Better CMS: administrator", IsSystematic = 1, IsDeleted = 0 };
            var u4 = new { Description = "Can manage Better CMS settings.", ModifiedOn = DateTime.Now, ModifiedByUser = "Admin" };

            Update.Table("Roles").InSchema(SchemaName).Set(u1).Where(w11);
            Update.Table("Roles").InSchema(SchemaName).Set(u1).Where(w12);

            Update.Table("Roles").InSchema(SchemaName).Set(u2).Where(w21);
            Update.Table("Roles").InSchema(SchemaName).Set(u2).Where(w22);

            Update.Table("Roles").InSchema(SchemaName).Set(u3).Where(w31);
            Update.Table("Roles").InSchema(SchemaName).Set(u3).Where(w32);

            Update.Table("Roles").InSchema(SchemaName).Set(u4).Where(w41);
            Update.Table("Roles").InSchema(SchemaName).Set(u4).Where(w42);
        }
    }
}