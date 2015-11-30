// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201401080830.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Search.Models.Migrations
{
    [Migration(201401080830)]
    public class Migration201401080830: DefaultMigration
    {
        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// The better CMS user name
        /// </summary>
        private const string BetterCmsUserName = "Better CMS";

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201401080830"/> class.
        /// </summary>
        public Migration201401080830()
            : base(SearchModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Migrates up current module.
        /// </summary>
        public override void Up()
        {
            var dateNow = DateTime.Now;

            var options = new
                    {
                        ResultsCount =
                            new
                                {
                                    Id = "6E0949B4-3339-4984-8103-D98F3FBC031D",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    ContentId = "663A1D0C-FADA-4ACC-A34F-7437523AE65B",
                                    Key = SearchModuleConstants.WidgetOptionNames.ResultsCount,
                                    Type = 2, // Integer
                                    DefaultValue = SearchModuleConstants.DefaultSearchResultsCount,
                                    IsDeletable = false
                                },
                        ShowTotalResults =
                            new
                                {
                                    Id = "33384E55-1B47-4040-A104-165F2979A7A0",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    ContentId = "663A1D0C-FADA-4ACC-A34F-7437523AE65B",
                                    Key = SearchModuleConstants.WidgetOptionNames.ShowTotalResults,
                                    Type = 5, // Boolean
                                    DefaultValue = "true",
                                    IsDeletable = false
                                }
                    };

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.ResultsCount)
                .Row(options.ShowTotalResults);
        }
    }
}