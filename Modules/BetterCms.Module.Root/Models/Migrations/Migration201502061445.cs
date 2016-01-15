// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201502061445.cs" company="Devbridge Group LLC">
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
    /// Categories: created parent category id and macro.
    /// </summary>
    [Migration(201502061445)]
    public class Migration201502061445 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201502041430"/> class.
        /// </summary>
        public Migration201502061445()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create
                .Table("WidgetCategories")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("WidgetId").AsGuid().NotNullable()
                .WithColumn("CategoryId").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_WidgetCategories_WidgetId_Cms_Widget_Id")
                .FromTable("WidgetCategories").InSchema(SchemaName).ForeignColumn("WidgetId")
                .ToTable("Widgets").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_WidgetCategories_CategoryId_Cms_Category_Id")
                .FromTable("WidgetCategories").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_WidgetCategories_WidgetId")
                .OnTable("WidgetCategories").InSchema(SchemaName).OnColumn("WidgetId");

            Create
                .Index("IX_Cms_WidgetCategories_CategoryId")
                .OnTable("WidgetCategories").InSchema(SchemaName).OnColumn("CategoryId");


            IfSqlServer().Execute.EmbeddedScript("Migration201502061445.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);

        }
    }
}