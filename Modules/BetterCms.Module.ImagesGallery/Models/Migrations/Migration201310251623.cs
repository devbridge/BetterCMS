// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201310251623.cs" company="Devbridge Group LLC">
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

using FluentMigrator;

namespace BetterCms.Module.ImagesGallery.Models.Migrations
{
    [Migration(201310251623)]
    public class Migration201310251623: DefaultMigration
    {
        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310251623" /> class.
        /// </summary>
        public Migration201310251623()
            : base(ImagesGalleryModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Migrates UP.
        /// </summary>
        public override void Up()
        {
            Update.Table("Contents")
                  .InSchema(rootSchemaName)
                  .Set(new { PreviewUrl = "/file/bcms-images-gallery/Content/GalleryPreview.png", Name = "Gallery Albums Widget" })
                  .Where(new { Id = "8BD1E830-6C2D-4AF1-BD5C-A24400A83DBC" });

            Update.Table("Contents")
                  .InSchema(rootSchemaName)
                  .Set(new { PreviewUrl = "/file/bcms-images-gallery/Content/GalleryPreview.png", Name = "Gallery Album Images Widget" })
                  .Where(new { Id = "F67BC85F-83A7-427E-85C5-A24D008B32E1" });
        }
    }
}