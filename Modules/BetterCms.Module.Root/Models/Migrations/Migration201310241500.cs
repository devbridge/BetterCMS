// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201310241500.cs" company="Devbridge Group LLC">
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

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201310241500)]
    public class Migration201310241500: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310241500"/> class.
        /// </summary>
        public Migration201310241500()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Set layout id as nullable
            Alter
                .Column("LayoutId")
                .OnTable("Pages").InSchema(SchemaName)
                .AsGuid().Nullable();

            // Create nullable master page id
            Create
                .Column("MasterPageId")
                .OnTable("Pages").InSchema(SchemaName)
                .AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_Pages_Cms_Pages")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("MasterPageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            // Create content regions table
            Create
                .Table("ContentRegions").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("RegionId").AsGuid().NotNullable()
                .WithColumn("ContentId").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_ContentRegions_Cms_Contents")
                .FromTable("ContentRegions").InSchema(SchemaName).ForeignColumn("ContentId")
                .ToTable("Contents").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_ContentRegions_Cms_Regions")
                .FromTable("ContentRegions").InSchema(SchemaName).ForeignColumn("RegionId")
                .ToTable("Regions").InSchema(SchemaName).PrimaryColumn("Id");

            // Create flag, indicating, that page is master page
            Create
                .Column("IsMasterPage")
                .OnTable("Pages").InSchema(SchemaName)
                .AsBoolean().NotNullable().WithDefaultValue(false);

            // Create master pages table
            Create
                .Table("MasterPages").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("MasterPageId").AsGuid().NotNullable();

            Create
                .UniqueConstraint("UX_Cms_MasterPages_PageId_MasterPageId")
                .OnTable("MasterPages").WithSchema(SchemaName)
                .Columns(new[] { "PageId", "MasterPageId", "DeletedOn" });

            Create
                .ForeignKey("FK_Cms_MasterPages_PageId_Cms_Pages")
                .FromTable("MasterPages").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_MasterPages_MasterPageId_Cms_Pages")
                .FromTable("MasterPages").InSchema(SchemaName).ForeignColumn("MasterPageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");
        }       
    }
}