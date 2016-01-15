// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitialSetup.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.ImagesGallery.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201309271415)]
    public class InitialSetup : DefaultMigration
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
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(ImagesGalleryModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
            pagesSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateWidget();
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Creates the widget.
        /// </summary>
        private void CreateWidget()
        {
            var widget = new
            {

                ForRootSchemaContentTable = new
                {
                    Id = "8BD1E830-6C2D-4AF1-BD5C-A24400A83DBC",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    Name = "Images Gallery Widget",
                    Status = 3,
                    PublishedOn = DateTime.Now,
                    PublishedByUser = "Better CMS"
                },

                ForRootScemaWidgetsTable = new
                {
                    Id = "8BD1E830-6C2D-4AF1-BD5C-A24400A83DBC",
                },

                ForPagesSchemaServerControlWidgetsTable = new
                {
                    Id = "8BD1E830-6C2D-4AF1-BD5C-A24400A83DBC",
                    Url = "~/Areas/bcms-images-gallery/Views/Widgets/ImagesGalleryWidget.cshtml"
                }

            };

            var options = new
            {
                Option1 = new
                {
                    Id = "309EDA84-6307-4619-8602-A2460080DE27",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = widget.ForRootSchemaContentTable.Id,
                    Key = "LoadCmsStyles",
                    Type = 5, // Boolean
                    DefaultValue = "true",
                    IsDeletable = false
                }
            };

            // Register server control widget.
            Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(widget.ForRootSchemaContentTable);
            Insert.IntoTable("Widgets").InSchema(rootSchemaName).Row(widget.ForRootScemaWidgetsTable);
            Insert.IntoTable("ServerControlWidgets").InSchema(pagesSchemaName).Row(widget.ForPagesSchemaServerControlWidgetsTable);

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.Option1);
        }
    }
}