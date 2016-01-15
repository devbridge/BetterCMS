// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201302211227.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201302211227)]
    public class Migration201302211227: DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201302211227"/> class.
        /// </summary>
        public Migration201302211227()
            : base(RootModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            // Create new column in pages
            Create
                .Column("Status")
                .OnTable("Pages").InSchema(rootModuleSchemaName)
                .AsInt32().NotNullable().WithDefaultValue(3);
            
            // Drop column from page properties
            Delete
                .Column("IsPublished")
                .FromTable("Pages").InSchema(SchemaName);
        }
    }
}