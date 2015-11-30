// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201308060930.cs" company="Devbridge Group LLC">
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
    [Migration(201308060930)]
    public class Migration201308060930: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308060930" /> class.
        /// </summary>
        public Migration201308060930()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Insert new option types to option types table
            Insert
                .IntoTable("OptionTypes")
                .InSchema(SchemaName)
                .Row(new { Id = 2, Name = "Integer" });
            
            Insert
                .IntoTable("OptionTypes")
                .InSchema(SchemaName)
                .Row(new { Id = 3, Name = "Float" });
            
            Insert
                .IntoTable("OptionTypes")
                .InSchema(SchemaName)
                .Row(new { Id = 4, Name = "DateTime" });
            
            Insert
                .IntoTable("OptionTypes")
                .InSchema(SchemaName)
                .Row(new { Id = 5, Name = "Boolean" });
        }
    }
}