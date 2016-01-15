// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201310160818.cs" company="Devbridge Group LLC">
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
    [Migration(201310160818)]
    public class Migration201310160818: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310160818"/> class.
        /// </summary>
        public Migration201310160818()
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

            Alter
                .Table("Pages").InSchema(SchemaName)
                .AlterColumn("PageUrl").AsString(MaxLength.Url).NotNullable();

            Alter
                .Table("Pages").InSchema(SchemaName)
                .AlterColumn("PageUrlLowerTrimmed").AsString(MaxLength.Url).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_Pages_PageUrl")
                .OnTable("Pages").WithSchema(SchemaName)
                .Columns(new[] { "PageUrl", "DeletedOn" });

            Create
                .Index("IX_Cms_Pages_PageUrl")
                .OnTable("Pages").InSchema(SchemaName)
                .OnColumn("PageUrl").Ascending();

            Create
                .UniqueConstraint("UX_Cms_Pages_PageUrlLowerTrimmed")
                .OnTable("Pages").WithSchema(SchemaName)
                .Columns(new[] { "PageUrlLowerTrimmed", "DeletedOn" });

            Create
                .Index("IX_Cms_Pages_PageUrlLowerTrimmed")
                .OnTable("Pages").InSchema(SchemaName)
                .OnColumn("PageUrlLowerTrimmed").Ascending();
        }       
    }
}