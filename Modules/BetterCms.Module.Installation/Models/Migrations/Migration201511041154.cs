// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201511041154.cs" company="Devbridge Group LLC">
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
    [Migration(201511041154)]
    public class Migration201511041154 : DefaultMigration
    {
        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201511041154"/> class.
        /// </summary>
        public Migration201511041154()
            : base(InstallationModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateDisqusWidgetOptions();
        }

        /// <summary>
        /// Creates the widget options.
        /// </summary>
        private void CreateDisqusWidgetOptions()
        {
            var options = new
            {
                DisqusCategoryId = new
                {
                    Id = "111CD45E-1CEB-4AD7-96F6-D00D68EA25CC",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = "2DFA000C-5FFE-45FF-98C9-320A942D86EF",
                    Key = "DisqusCategoryId",
                    Type = 1, // Text
                    DefaultValue = String.Empty,
                    IsDeletable = false
                },
                DisqusShortName = new
                {
                    Id = "4742CDBC-54AA-4C38-9738-A42BDE1FFE95",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = "2DFA000C-5FFE-45FF-98C9-320A942D86EF",
                    Key = "DisqusShortName",
                    Type = 1, // Text
                    DefaultValue = String.Empty,
                    IsDeletable = false
                }
            };

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.DisqusCategoryId);
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.DisqusShortName);
        }
    }
}