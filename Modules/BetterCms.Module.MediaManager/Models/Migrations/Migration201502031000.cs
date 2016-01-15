// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201502031000.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Root.Models.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201502031000)]
    public class Migration201502031000 : DefaultMigration
    {
        private readonly string rootModuleSchemaName;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="201502031000"/> class.
        /// </summary>
        public Migration201502031000()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Table("MediaCategories")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("MediaId").AsGuid().NotNullable()
                .WithColumn("CategoryId").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_MediaCategories_MediaId_Cms_Mdedia_Id")
                .FromTable("MediaCategories").InSchema(SchemaName).ForeignColumn("MediaId")
                .ToTable("Medias").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_MediaCategories_CategoryId_Cms_Category_Id")
                .FromTable("MediaCategories").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(rootModuleSchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_MediaCategories_MediaId")
                .OnTable("MediaCategories").InSchema(SchemaName).OnColumn("MediaId");

            Create
                .Index("IX_Cms_MediaCategories_AccessRuleId")
                .OnTable("MediaCategories").InSchema(SchemaName).OnColumn("CategoryId");
        }        
    }
}