// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201309040830.cs" company="Devbridge Group LLC">
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
    [Migration(201309040830)]
    public class Migration201309040830: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201309040830"/> class.
        /// </summary>
        public Migration201309040830()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Add column IsDeletable to option tables
            Alter
                .Table("LayoutOptions").InSchema(SchemaName)
                .AddColumn("IsDeletable")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(true);
            
            Alter
                .Table("ContentOptions").InSchema(SchemaName)
                .AddColumn("IsDeletable")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(true);
        }       
    }
}