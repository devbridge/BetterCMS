// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201306201215.cs" company="Devbridge Group LLC">
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
using System.Data.SqlTypes;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201306201215)]
    public class Migration201306201215: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306201215"/> class.
        /// </summary>
        public Migration201306201215()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Column("PublishedOn")
                .OnTable("Medias").InSchema(SchemaName)
                .AsDateTime().Nullable();

            Update
                .Table("Medias").InSchema(SchemaName)
                .Set(new { PublishedOn = (DateTime)SqlDateTime.MinValue })
                .AllRows();

            Alter
               .Column("PublishedOn")
               .OnTable("Medias").InSchema(SchemaName)
               .AsDateTime().NotNullable();
        }
    }
}