// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201306101421.cs" company="Devbridge Group LLC">
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
using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201306101421)]
    public class Migration201306101421: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306101421"/> class.
        /// </summary>
        public Migration201306101421() : base(RootModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            // Unpublish hidden pages.
            Update
                .Table("Pages")
                .InSchema(SchemaName)
                .Set(new { Status = 4 })
                .Where(new { Status = 3, IsPublic = 0 });

            Delete
                .Column("IsPublic")
                .FromTable("Pages")
                .InSchema(SchemaName);
        }
    }
}