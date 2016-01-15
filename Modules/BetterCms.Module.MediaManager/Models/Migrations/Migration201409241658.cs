// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201409241658.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Root.Models.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201409241658)]
    public class Migration201409241658: DefaultMigration
    {
        private readonly string rootModuleSchemaName;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="201409241658"/> class.
        /// </summary>
        public Migration201409241658()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {            
            // Create custom option for selecting a media folder
            Insert
               .IntoTable("CustomOptions").InSchema(rootModuleSchemaName)
               .Row(new
                   {
                       Id = new Guid("d88604ee-d3d5-4e0c-b071-984143a9ed74"),
                       Version = 1,
                       CreatedOn = DateTime.Now,
                       ModifiedOn = DateTime.Now,
                       CreatedByUser = "Admin",
                       ModifiedByUser = "Admin",
                       IsDeleted = 0,
                       Identifier = "media-images-url",
                       Title = "Image URL"
                   });
        }        
    }
}