// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201308272137.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201308272137)]
    public class Migration201308272137: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308252344"/> class.
        /// </summary>
        public Migration201308272137()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateAccessRulesTable();
            CreatePageAccessRulesTable();
        }

        private void CreateAccessRulesTable()
        {
            Create
                .Table("AccessRules")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("Identity").AsString(MaxLength.Name).NotNullable()
                .WithColumn("AccessLevel").AsInt32().NotNullable();
        }

        private void CreatePageAccessRulesTable()
        {
            if (Schema.Schema(SchemaName).Table("PageAccess").Exists())
            {
                Delete.Index("IX_Cms_PageAccess_PageId").OnTable("PageAccess").InSchema(SchemaName);
                Delete.ForeignKey("FK_Cms_PageAccess_PageId_Cms_Page_Id").OnTable("PageAccess").InSchema(SchemaName);
                Delete.Table("PageAccess").InSchema(SchemaName);
            }

            Create
                .Table("PageAccessRules")
                .InSchema(SchemaName)
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("AccessRuleId").AsGuid().NotNullable();

            Create.PrimaryKey("PK_Cms_PageAccessRules").OnTable("PageAccessRules").WithSchema(SchemaName).Columns(new[] { "PageId", "AccessRuleId" });

            Create
                .ForeignKey("FK_Cms_PageAccessRules_PageId_Cms_Page_Id")
                .FromTable("PageAccessRules").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageAccessRules_AccessRuleId_Cms_AccessRules_Id")
                .FromTable("PageAccessRules").InSchema(SchemaName).ForeignColumn("AccessRuleId")
                .ToTable("AccessRules").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_PageAccessRules_PageId")
                .OnTable("PageAccessRules").InSchema(SchemaName).OnColumn("PageId");

            Create
                .Index("IX_Cms_PageAccessRules_AccessRuleId")
                .OnTable("PageAccessRules").InSchema(SchemaName).OnColumn("AccessRuleId");
        }
    }
}