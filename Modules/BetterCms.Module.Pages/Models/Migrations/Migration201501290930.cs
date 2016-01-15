// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201501290930.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Pages.Models.Migrations
{
    [Migration(201501290930)]
    public class Migration201501290930 : DefaultMigration
    {
        private readonly string rootModuleSchemaName;
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201501290930"/> class.
        /// </summary>
        public Migration201501290930()
            : base(PagesModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            CreatePageAccessRulesTable();
        }
        private void CreatePageAccessRulesTable()
        {
            Create
                .Table("PageCategories")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("CategoryId").AsGuid().NotNullable();           

            Create
                .ForeignKey("FK_Cms_PageCategories_PageId_Cms_Page_Id")
                .FromTable("PageCategories").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageCategories_CategoryId_Cms_Category_Id")
                .FromTable("PageCategories").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(rootModuleSchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_PageCategories_PageId")
                .OnTable("PageCategories").InSchema(SchemaName).OnColumn("PageId");

            Create
                .Index("IX_Cms_PageCategories_AccessRuleId")
                .OnTable("PageCategories").InSchema(SchemaName).OnColumn("CategoryId");
        }
    }
}