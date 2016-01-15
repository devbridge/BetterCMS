// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201407231054.cs" company="Devbridge Group LLC">
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
    /// <summary>
    /// Child contents database structure create.
    /// </summary>
    [Migration(201407231054)]
    public class Migration201407231054: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201407231054"/> class.
        /// </summary>
        public Migration201407231054()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Create table.
            Create
                .Table("ProtocolForcingTypes")
                .InSchema(SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            // Create Uq.
            Create
                .UniqueConstraint("UX_Cms_ProtocolForcingTypes_Name")
                .OnTable("ProtocolForcingTypes").WithSchema(SchemaName)
                .Column("Name");

            // Insert page access protocols.
            Insert
                .IntoTable("ProtocolForcingTypes")
                .InSchema(SchemaName)
                .Row(new
                {
                    Id = 0,
                    Name = "None"
                })
                .Row(new
                {
                    Id = 1,
                    Name = "ForceHttp"
                })
                .Row(new
                {
                    Id = 2,
                    Name = "ForceHttps"
                });

            // Create new column.
            Create
                .Column("ForceAccessProtocol")
                .OnTable("Pages").InSchema(SchemaName)
                .AsInt32()
                .WithDefaultValue(0)
                .NotNullable();

            // Create FK.
            Create
                .ForeignKey("FK_Cms_RootPages_Cms_ProtocolForcingTypes")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("ForceAccessProtocol")
                .ToTable("ProtocolForcingTypes").InSchema(SchemaName).PrimaryColumn("Id");
        }
    }
}