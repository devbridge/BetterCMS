// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201310081330.cs" company="Devbridge Group LLC">
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
    /// Deletes custom option and table, if exists
    /// </summary>
    [Migration(201310081330)]
    public class Migration201310081330: DefaultMigration
    {
        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310081330" /> class.
        /// </summary>
        public Migration201310081330()
            : base(ImagesGalleryModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Migrates UP.
        /// </summary>
        public override void Up()
        {
            DeleteUnusedTableAndOptions();
        }
        
        /// <summary>
        /// Deletes custom option and table, if exists.
        /// </summary>
        private void DeleteUnusedTableAndOptions()
        {
            // Deletes albums table if such exists
            if (Schema.Schema(SchemaName).Table("Albums").Exists())
            {
                Delete.Table("Albums").InSchema(SchemaName);
            }

            // Deletes custom option
            Update
                .Table("CustomOptions")
                .InSchema(rootSchemaName)
                .Set(new
                         {
                             IsDeleted = 1,
                             DeletedOn = DateTime.Now,
                             DeletedByUser = "Better CMS admin"
                         })
                .Where(new
                           {
                               IsDeleted = 0,
                               Id = new Guid("9BCDA77D-C900-4AED-96D9-4FE8AD1F4138")
                           });

            // Deletes content option value
            Update
                .Table("ContentOptions")
                .InSchema(rootSchemaName)
                .Set(new
                         {
                             IsDeleted = 1,
                             DeletedOn = DateTime.Now,
                             DeletedByUser = "Better CMS admin"
                         })
                .Where(new
                        {
                            IsDeleted = 0,
                            Id = new Guid("CD7CAD6B-1005-4E6F-B53A-A24D008C5C01")
                        });
        }
    }
}