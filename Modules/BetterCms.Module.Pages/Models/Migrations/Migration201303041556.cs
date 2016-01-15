// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201303041556.cs" company="Devbridge Group LLC">
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
    /// Database structure setup.
    /// </summary>
    [Migration(201303041556)]
    public class Migration201303041556: DefaultMigration
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201303041556"/> class.
        /// </summary>
        public Migration201303041556() : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// The up.
        /// </summary>
        public override void Up()
        {
            CreateSitemapNodesTable();            
        }

        /// <summary>
        /// The create sitemap nodes table.
        /// </summary>
        private void CreateSitemapNodesTable()
        {
            Create
               .Table("SitemapNodes")
               .InSchema(SchemaName)

               .WithBaseColumns()
               .WithColumn("Title").AsString(MaxLength.Name).NotNullable()
               .WithColumn("Url").AsString(MaxLength.Url).NotNullable()
               .WithColumn("DisplayOrder").AsInt32().NotNullable()
               .WithColumn("ParentNodeId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_SitemapNodes_ParentNodeId_SitemapNodes_Id")
                .FromTable("SitemapNodes").InSchema(SchemaName).ForeignColumn("ParentNodeId")
                .ToTable("SitemapNodes").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_SitemapNodes_Title")
                .OnTable("SitemapNodes").InSchema(SchemaName).OnColumn("Title").Ascending();

            Create
                .Index("IX_Cms_SitemapNodes_ParentNodeId")
                .OnTable("SitemapNodes").InSchema(SchemaName).OnColumn("ParentNodeId").Ascending();
        }
    }
}