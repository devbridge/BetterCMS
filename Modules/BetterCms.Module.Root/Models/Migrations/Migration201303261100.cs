// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201303261100.cs" company="Devbridge Group LLC">
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
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// module database structure update.
    /// </summary>
    [Migration(201303261100)]
    public class Migration201303261100: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201303261100"/> class.
        /// </summary>
        public Migration201303261100()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            // Set non valid statuses as unpublished
            Update
                .Table("Pages").InSchema(SchemaName)
                .Set(new { Status = 4 })
                .Where(new { Status = 0 });

            // Create table
            Create
                .Table("PageStatuses")
                .InSchema(SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            // Create Uq
            Create
                .UniqueConstraint("UX_Cms_PageStatuses_Name")
                .OnTable("PageStatuses").WithSchema(SchemaName)
                .Column("Name");

            // Insert page statuses
            Insert
                .IntoTable("PageStatuses")
                .InSchema(SchemaName)
                .Row(new
                {
                    Id = 1,
                    Name = "Preview"
                })
                .Row(new
                {
                    Id = 2,
                    Name = "Draft"
                })
                .Row(new
                {
                    Id = 3,
                    Name = "Published"
                })
                .Row(new
                {
                    Id = 4,
                    Name = "Unpublished"
                });

            // Create FK
            Create
                .ForeignKey("FK_Cms_RootPages_Cms_PageStatuses")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("Status")
                .ToTable("PageStatuses").InSchema(SchemaName).PrimaryColumn("Id");
        }
    }
}