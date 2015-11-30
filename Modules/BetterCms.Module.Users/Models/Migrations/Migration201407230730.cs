// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201407230730.cs" company="Devbridge Group LLC">
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