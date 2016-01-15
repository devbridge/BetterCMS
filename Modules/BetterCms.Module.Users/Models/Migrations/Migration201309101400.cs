// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201309101400.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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