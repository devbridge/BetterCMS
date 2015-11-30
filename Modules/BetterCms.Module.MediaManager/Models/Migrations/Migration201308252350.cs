// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201308252350.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201308252350)]
    public class Migration201308252350: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308252350"/> class.
        /// </summary>
        public Migration201308252350()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create
                 .Table("MediaFileAccess")
                 .InSchema(SchemaName)
                 .WithBaseColumns()
                 .WithColumn("MediaFileId").AsGuid().NotNullable()
                 .WithColumn("RoleOrUser").AsString(MaxLength.Name).NotNullable()
                 .WithColumn("AccessLevel").AsInt32().NotNullable();

            Create
                .ForeignKey("FK_Cms_MediaFileAccess_MediaFileId_Cms_MediaFile_Id")
                .FromTable("MediaFileAccess").InSchema(SchemaName).ForeignColumn("MediaFileId")
                .ToTable("MediaFiles").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_MediaFileAccess_MediaFileId")
                .OnTable("MediaFileAccess").InSchema(SchemaName).OnColumn("MediaFileId");
        }
    }
}