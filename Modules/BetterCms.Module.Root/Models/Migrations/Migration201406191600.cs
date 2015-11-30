// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201406191600.cs" company="Devbridge Group LLC">
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
    /// <summary>
    /// Child contents database structure create.
    /// </summary>
    [Migration(201406191600)]
    public class Migration201406191600: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201406191600"/> class.
        /// </summary>
        public Migration201406191600()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateChildContentsTable();
            CreateChildContentOptionsTable();
        }

        /// <summary>
        /// Creates the child contents table.
        /// </summary>
        private void CreateChildContentsTable()
        {
            Create
                .Table("ChildContents").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("ParentContentId").AsGuid().NotNullable()
                .WithColumn("ChildContentId").AsGuid().NotNullable()
                .WithColumn("AssignmentIdentifier").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_ChildContents_ParentContentId_Contents_Id")
                .FromTable("ChildContents").InSchema(SchemaName).ForeignColumn("ParentContentId")
                .ToTable("Contents").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_ChildContents_ChildContentId_Contents_Id")
                .FromTable("ChildContents").InSchema(SchemaName).ForeignColumn("ChildContentId")
                .ToTable("Contents").InSchema(SchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the child content options table.
        /// </summary>
        private void CreateChildContentOptionsTable()
        {
            Create
                .Table("ChildContentOptions")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("ChildContentId").AsGuid().NotNullable()
                .WithColumn("Value").AsString(MaxLength.Max).Nullable()
                .WithColumn("Key").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("CustomOptionId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_ChildContentOptions_ChildContentId_Cms_ChildContents_Id")
                .FromTable("ChildContentOptions").InSchema(SchemaName).ForeignColumn("ChildContentId")
                .ToTable("ChildContents").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_ChildContentOptions_Type_Cms_OptionTypes_Id")
                .FromTable("ChildContentOptions").InSchema(SchemaName).ForeignColumn("Type")
                .ToTable("OptionTypes").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .UniqueConstraint("UX_Cms_ChildContentOptions_ChildContentId_Key")
                .OnTable("ChildContentOptions").WithSchema(SchemaName)
                .Columns(new[] { "ChildContentId", "Key", "DeletedOn" });

            Create
                .ForeignKey("FK_Cms_ChildContentOptions_CustomOptionId_Cms_CustomOptions_Id")
                .FromTable("ChildContentOptions").InSchema(SchemaName).ForeignColumn("CustomOptionId")
                .ToTable("CustomOptions").InSchema(SchemaName).PrimaryColumn("Id");
        }
    }
}