// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201308051530.cs" company="Devbridge Group LLC">
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
    [Migration(201308051530)]
    public class Migration201308051530: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308051530" /> class.
        /// </summary>
        public Migration201308051530()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Create table for page options
            Create
                .Table("PageOptions")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("Value").AsString(MaxLength.Max).Nullable()
                .WithColumn("Key").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Type").AsInt32().NotNullable();

            Create
                .ForeignKey("FK_Cms_PageOptions_PageId_Cms_Pages_Id")
                .FromTable("PageOptions").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageOptions_Type_Cms_OptionTypes_Id")
                .FromTable("PageOptions").InSchema(SchemaName).ForeignColumn("Type")
                .ToTable("OptionTypes").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .UniqueConstraint("UX_Cms_PageOptions_PageId_Key")
                .OnTable("PageOptions").WithSchema(SchemaName)
                .Columns(new[] { "PageId", "Key", "DeletedOn" });
        }
    }
}