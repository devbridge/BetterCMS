// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201309091554.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201309091554)]
    public class Migration201309091554: DefaultMigration
    {
        private static readonly Guid loginWidgetId = new Guid("DE0E47B2-728D-4BE6-904D-ED99CDDEDA4A");

        private readonly string rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;

        public Migration201309091554()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }
        
        public override void Up()
        {
            Update.Table("Contents").InSchema(rootSchemaName)
                .Set(new { IsDeleted = true })
                .Where(new { Id = loginWidgetId });
        }
    }
}