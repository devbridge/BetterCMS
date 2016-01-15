// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201510291304.cs" company="Devbridge Group LLC">
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
using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Installation.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201510291304)]
    public class Migration201510291304 : DefaultMigration
    {
        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// The pages schema name
        /// </summary>
        private readonly string pagesSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201510291304"/> class.
        /// </summary>
        public Migration201510291304()
            : base(InstallationModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
            pagesSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateBlogCategoriesWidgetOptions();
        }

        /// <summary>
        /// Creates the widget options.
        /// </summary>
        private void CreateBlogCategoriesWidgetOptions()
        {
            var options = new
            {
                UseSpecificCategoryTree = new
                {
                    Id = "586B9316-4BA0-4C5D-B568-55E99E8C4E3F",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = "4110B9AA-8598-4A97-8EB6-A5830F6A43D1",
                    Key = "UseSpecificCategoryTree",
                    Type = 5, // Boolean
                    DefaultValue = "false",
                    IsDeletable = false
                },
                CategoryTreeName = new
                {
                    Id = "BA516B96-B389-4376-8B1C-CB152DDCCAFC",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = "4110B9AA-8598-4A97-8EB6-A5830F6A43D1",
                    Key = "CategoryTreeName",
                    Type = 1, // Text
                    DefaultValue = String.Empty,
                    IsDeletable = false
                }
            };

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.UseSpecificCategoryTree);
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.CategoryTreeName);
        }
    }
}