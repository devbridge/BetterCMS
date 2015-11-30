// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201401061130.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201401061130)]
    public class Migration201401061130: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201401061130"/> class.
        /// </summary>
        public Migration201401061130() : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Table("SitemapArchives")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("SitemapId").AsGuid().NotNullable()
                .WithColumn("Title").AsString(MaxLength.Name).NotNullable()
                .WithColumn("ArchivedVersion").AsString(MaxLength.Max).NotNullable();

            Create
                .ForeignKey("FK_Cms_SitemapArchives_Cms_Sitemaps")
                .FromTable("SitemapArchives").InSchema(SchemaName).ForeignColumn("SitemapId")
                .ToTable("Sitemaps").InSchema(SchemaName).PrimaryColumn("Id");
        }
    }
}