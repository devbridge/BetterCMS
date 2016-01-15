// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201310170919.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201310170919)]
    public class Migration201310170919: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310170919"/> class.
        /// </summary>
        public Migration201310170919()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Delete
                .UniqueConstraint("UX_Cms_Pages_PageUrl")
                .FromTable("Pages").InSchema(SchemaName);

            Delete
                .Index("IX_Cms_Pages_PageUrl")
                .OnTable("Pages").InSchema(SchemaName);

            Delete
                .UniqueConstraint("UX_Cms_Pages_PageUrlLowerTrimmed")
                .FromTable("Pages").InSchema(SchemaName);

            Delete
                .Index("IX_Cms_Pages_PageUrlLowerTrimmed")
                .OnTable("Pages").InSchema(SchemaName);

            Delete
                .Column("PageUrlLowerTrimmed")
                .FromTable("Pages").InSchema(SchemaName);
                
            Alter
                .Table("Pages").InSchema(SchemaName)
                .AddColumn("PageUrlHash").AsAnsiString(MaxLength.UrlHash).Nullable();

            IfSqlServer().Execute.EmbeddedScript("Migration201310170919.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);

            Alter
                .Table("Pages").InSchema(SchemaName)
                .AlterColumn("PageUrlHash").AsAnsiString(MaxLength.UrlHash).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_Pages_PageUrlHash")
                .OnTable("Pages").WithSchema(SchemaName)
                .Columns(new[] { "PageUrlHash", "DeletedOn" });

            Create
                .Index("IX_Cms_Pages_PageUrlHash")
                .OnTable("Pages").InSchema(SchemaName)
                .OnColumn("PageUrlHash").Ascending();
        }       
    }
}