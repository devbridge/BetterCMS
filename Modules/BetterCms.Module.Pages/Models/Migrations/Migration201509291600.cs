// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201509291600.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201509291600)]
    public class Migration201509291600 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201509291600"/> class.
        /// </summary>
        public Migration201509291600()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Alter.Table("HtmlContents").InSchema(SchemaName)
                .AddColumn("OriginalText").AsString(MaxLength.Max).Nullable()
                .AddColumn("ContentTextMode").AsInt32().NotNullable().WithDefaultValue("1");

            Create
                .Table("ContentTextModes")
                .InSchema(SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_ContentTextModes_Name")
                .OnTable("ContentTextModes").WithSchema(SchemaName)
                .Column("Name");

            Insert
                .IntoTable("ContentTextModes")
                .InSchema(SchemaName)
                .Row(new
                {
                    Id = 1,
                    Name = "Html"
                })
                .Row(new
                {
                    Id = 2,
                    Name = "Markdown"
                })
                .Row(new
                {
                    Id = 3,
                    Name = "SimpleText"
                });

            Create
                .ForeignKey("FK_Cms_HtmlContents_ContentTextMode_ContentTextModes_Id")
                .FromTable("HtmlContents").InSchema(SchemaName).ForeignColumn("ContentTextMode")
                .ToTable("ContentTextModes").InSchema(SchemaName).PrimaryColumn("Id");
        }
    }
}