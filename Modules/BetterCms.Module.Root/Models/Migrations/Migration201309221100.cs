// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201309221100.cs" company="Devbridge Group LLC">
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
    [Migration(201309221100)]
    public class Migration201309221100: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201309221100"/> class.
        /// </summary>
        public Migration201309221100()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Add custom option to option types table
            Insert
                .IntoTable("OptionTypes").InSchema(SchemaName)
                .Row(new { Id = 99, Name = "Custom" });

            // Create custom options table
            Create
                .Table("CustomOptions")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("Title").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Identifier").AsString(MaxLength.Name).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_CustomOptions_Identifier")
                .OnTable("CustomOptions").WithSchema(SchemaName)
                .Columns(new[] { "Identifier", "DeletedOn" });

            // Add custom option references to all option tables
            Create
                .Column("CustomOptionId")
                .OnTable("ContentOptions").InSchema(SchemaName)
                .AsGuid().Nullable();

            Create
                .Column("CustomOptionId")
                .OnTable("PageContentOptions").InSchema(SchemaName)
                .AsGuid().Nullable();

            Create
                .Column("CustomOptionId")
                .OnTable("LayoutOptions").InSchema(SchemaName)
                .AsGuid().Nullable();
            
            Create
                .Column("CustomOptionId")
                .OnTable("PageOptions").InSchema(SchemaName)
                .AsGuid().Nullable();

            // Create foreign keys for custom options
            Create
                .ForeignKey("FK_Cms_ContentOptions_CustomOptionId_Cms_CustomOptions_Id")
                .FromTable("ContentOptions").InSchema(SchemaName).ForeignColumn("CustomOptionId")
                .ToTable("CustomOptions").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageContentOptions_CustomOptionId_Cms_CustomOptions_Id")
                .FromTable("PageContentOptions").InSchema(SchemaName).ForeignColumn("CustomOptionId")
                .ToTable("CustomOptions").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_LayoutOptions_CustomOptionId_Cms_CustomOptions_Id")
                .FromTable("LayoutOptions").InSchema(SchemaName).ForeignColumn("CustomOptionId")
                .ToTable("CustomOptions").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageOptions_CustomOptionId_Cms_CustomOptions_Id")
                .FromTable("PageOptions").InSchema(SchemaName).ForeignColumn("CustomOptionId")
                .ToTable("CustomOptions").InSchema(SchemaName).PrimaryColumn("Id");
        }       
    }
}