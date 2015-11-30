// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201510261332.cs" company="Devbridge Group LLC">
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
    [Migration(201510261332)]
    public class Migration201510261332 : DefaultMigration
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
        /// Initializes a new instance of the <see cref="Migration201510261332"/> class.
        /// </summary>
        public Migration201510261332()
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
            CreateBlogPostsWidget();
        }

        /// <summary>
        /// Creates the widget.
        /// </summary>
        private void CreateBlogPostsWidget()
        {
            var widget = new
            {
                ForRootSchemaCategoriesTable = new
                {
                    Id = "973B0FA7-4633-4154-BA5D-49BCE5591CC0",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    Name = "Blog",
                    CategoryTreeId = "98FD87B4-A25C-4DDE-933C-83826B6A94D7"
                },

                ForRootSchemaContentTable = new
                {
                    Id = "7218F62A-6C38-49EA-AF65-D610E4B3AE0A",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    Name = "Blog Posts List Widget",
                    Status = 3,
                    PublishedOn = DateTime.Now,
                    PublishedByUser = "Better CMS"
                },

                ForRootScemaWidgetsTable = new
                {
                    Id = "7218F62A-6C38-49EA-AF65-D610E4B3AE0A",
                    CategoryId = "973B0FA7-4633-4154-BA5D-49BCE5591CC0"
                },

                ForPagesSchemaServerControlWidgetsTable = new
                {
                    Id = "7218F62A-6C38-49EA-AF65-D610E4B3AE0A",
                    Url = "~/Areas/bcms-installation/Views/Widgets/BlogPostsWidgetInvoker.cshtml"
                }

            };

            var options = new
            {
                ShowAuthor = new
                {
                    Id = "45BD4A45-9F2E-4FA1-9E2F-0DD99A7DE343",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = widget.ForRootSchemaContentTable.Id,
                    Key = "ShowAuthor",
                    Type = 5, // Boolean
                    DefaultValue = "true",
                    IsDeletable = false
                },
                ShowDate = new
                {
                    Id = "8FCED5BD-DE4D-492E-B04A-FA3BA83CAEEC",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = widget.ForRootSchemaContentTable.Id,
                    Key = "ShowDate",
                    Type = 5, // Boolean
                    DefaultValue = "true",
                    IsDeletable = false
                },
                ShowTags = new
                {
                    Id = "E39392C8-37B9-4506-B810-B3D38A0F022F",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = widget.ForRootSchemaContentTable.Id,
                    Key = "ShowTags",
                    Type = 5, // Boolean
                    DefaultValue = "true",
                    IsDeletable = false
                },
                ShowCategories = new
                {
                    Id = "4AE831E6-71A7-4280-9FB9-32A37F397035",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = widget.ForRootSchemaContentTable.Id,
                    Key = "ShowCategories",
                    Type = 5, // Boolean
                    DefaultValue = "true",
                    IsDeletable = false
                },
                ShowPager = new
                {
                    Id = "9D888B08-604E-4E7D-A9E9-4B4A5C9E5514",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = widget.ForRootSchemaContentTable.Id,
                    Key = "ShowPager",
                    Type = 5, // Boolean
                    DefaultValue = "true",
                    IsDeletable = false
                },
                PageSize = new
                {
                    Id = "D1960653-1ABB-41FD-8370-BAA5CA243499",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = widget.ForRootSchemaContentTable.Id,
                    Key = "PageSize",
                    Type = 2, // Integer
                    DefaultValue = 6,
                    IsDeletable = false
                }
            };

            // Register server control widget.
            Insert.IntoTable("Categories").InSchema(rootSchemaName).Row(widget.ForRootSchemaCategoriesTable);
            Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(widget.ForRootSchemaContentTable);
            Insert.IntoTable("Widgets").InSchema(rootSchemaName).Row(widget.ForRootScemaWidgetsTable);
            Insert.IntoTable("ServerControlWidgets").InSchema(pagesSchemaName).Row(widget.ForPagesSchemaServerControlWidgetsTable);

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.ShowAuthor);
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.ShowDate);
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.ShowCategories);
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.ShowTags);
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.ShowPager);
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.PageSize);
        }
    }
}