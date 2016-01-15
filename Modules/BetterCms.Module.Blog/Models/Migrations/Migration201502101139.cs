// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201502101139.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Blog.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201502101139)]
    public class Migration201502101139 : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201502101139"/> class.
        /// </summary>
        public Migration201502101139() : base(BlogModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            if (Schema.Schema(rootModuleSchemaName).Table("CategorizableItems").Exists())
            {
                Insert.IntoTable("CategorizableItems").InSchema(rootModuleSchemaName).Row(new
                {

                    Id = "75E6C021-1D1F-459E-A416-D18477BF2020",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    Name = BlogPost.CategorizableItemKeyForBlogs
                });
            }

            MigrateCategoryData();
        }

        private void MigrateCategoryData()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201502101139.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }
    }
}