// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201501131230.cs" company="Devbridge Group LLC">
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
using System;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Categories: created parent category id and macro.
    /// </summary>
    [Migration(201501131230)]
    public class Migration201501131230 : DefaultMigration
    {
        /// <summary>
        /// The default Category Tree identifier.
        /// </summary>
        private const string DefaultCategoryTreeId = "98FD87B4-A25C-4DDE-933C-83826B6A94D7";

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201501131230"/> class.
        /// </summary>
        public Migration201501131230()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            if (!Schema.Schema(SchemaName).Table("Categories").Column("ParentCategoryId").Exists())
            {
                Create
                    .Column("ParentCategoryId")
                    .OnTable("Categories").InSchema(SchemaName)
                    .AsGuid()
                    .Nullable();

                Create
                    .ForeignKey("Fk_Cms_Categories_ParentCategoryId__Categories_Id")
                    .FromTable("Categories").InSchema(SchemaName).ForeignColumn("ParentCategoryId")
                    .ToTable("Categories").InSchema(SchemaName).PrimaryColumn("Id");
            }

            if (!Schema.Schema(SchemaName).Table("Categories").Column("DisplayOrder").Exists())
            {
                Create
                    .Column("DisplayOrder")
                    .OnTable("Categories").InSchema(SchemaName)
                    .AsInt32().NotNullable().WithDefaultValue(0);
            }

            if (!Schema.Schema(SchemaName).Table("Categories").Column("Macro").Exists())
            {
                Create
                    .Column("Macro")
                    .OnTable("Categories").InSchema(SchemaName)
                    .AsString(MaxLength.Text).Nullable();
            }
 
            if (!Schema.Schema(SchemaName).Table("CategoryTrees").Exists())
            {
                Create
                    .Table("CategoryTrees").InSchema(SchemaName)
                    .WithBaseColumns()
                    .WithColumn("Title").AsString(MaxLength.Name).NotNullable();

                CreateDefaultCategoryTree();
            }

            if (!Schema.Schema(SchemaName).Table("Categories").Column("CategoryTreeId").Exists())
            {
                Create
                    .Column("CategoryTreeId")
                    .OnTable("Categories").InSchema(SchemaName)
                    .AsGuid().Nullable();

                Update
                    .Table("Categories").InSchema(SchemaName)
                    .Set(new { CategoryTreeId = DefaultCategoryTreeId })
                    .AllRows();
                
                Alter
                    .Table("Categories").InSchema(SchemaName)
                    .AlterColumn("CategoryTreeId")
                    .AsGuid().NotNullable();

                Create
                    .ForeignKey("FK_Cms_Categories_CategoryTreeId_Cms_CategoryTree_Id")
                    .FromTable("Categories").InSchema(SchemaName).ForeignColumn("CategoryTreeId")
                    .ToTable("CategoryTrees").InSchema(SchemaName).PrimaryColumn("Id");

                Create
                    .Index("IX_Cms_Categories_CategoryTreeId")
                    .OnTable("Categories").InSchema(SchemaName).OnColumn("CategoryTreeId");
            }
        }

        private void CreateDefaultCategoryTree()
        {
            Insert.IntoTable("CategoryTrees")
                .InSchema(SchemaName)
                .Row(
                    new
                      {
                          Id = DefaultCategoryTreeId,
                          Version = 1,
                          IsDeleted = false,
                          CreatedOn = DateTime.Now,
                          CreatedByUser = "Better CMS",
                          ModifiedOn = DateTime.Now,
                          ModifiedByUser = "Better CMS",
                          Title = "Default Category Tree"
                      });
        }
    }
}