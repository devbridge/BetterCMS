// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201502101035.cs" company="Devbridge Group LLC">
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
    /// Categories: created parent category id and macro.
    /// </summary>
    [Migration(201502101035)]
    public class Migration201502101035 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201502101035"/> class.
        /// </summary>
        public Migration201502101035()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            if (!Schema.Schema(SchemaName).Table("CategorizableItems").Exists())
            {
                Create
                .Table("CategorizableItems").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

                Create
                    .UniqueConstraint("UX_Cms_CategorizableItems_Name")
                    .OnTable("CategorizableItems").WithSchema(SchemaName)
                    .Columns(new[] { "Name", "DeletedOn" });
            }

            if (!Schema.Schema(SchemaName).Table("CategoryTreeCategorizableItems").Exists())
            {
                Create
               .Table("CategoryTreeCategorizableItems")
               .InSchema(SchemaName)
               .WithBaseColumns()
               .WithColumn("CategoryTreeId").AsGuid().NotNullable()
               .WithColumn("CategorizableItemId").AsGuid().NotNullable();

                Create
                    .ForeignKey("FK_Cms_CategoryTreeCategorizableItems_Cms_CategoryTrees")
                    .FromTable("CategoryTreeCategorizableItems").InSchema(SchemaName).ForeignColumn("CategoryTreeId")
                    .ToTable("CategoryTrees").InSchema(SchemaName).PrimaryColumn("Id");

                Create
                    .ForeignKey("FK_Cms_CategoryTreeCategorizableItems_Cms_CategorizableItems")
                    .FromTable("CategoryTreeCategorizableItems").InSchema(SchemaName).ForeignColumn("CategorizableItemId")
                    .ToTable("CategorizableItems").InSchema(SchemaName).PrimaryColumn("Id");
            }
        }
    }
}