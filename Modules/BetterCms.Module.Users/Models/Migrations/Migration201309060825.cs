// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201309060825.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201309060825)]
    public class Migration201309060825: DefaultMigration
    {
        public Migration201309060825()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }
        
        public override void Up()
        {
            // Users table's columns FirstName and LastName are not mandatory anymore.
            Alter.Table("Users").InSchema(SchemaName).AlterColumn("FirstName").AsString(MaxLength.Name).Nullable();
            Alter.Table("Users").InSchema(SchemaName).AlterColumn("LastName").AsString(MaxLength.Name).Nullable();

            // Drop unique constraints
            if (Schema.Schema(SchemaName).Table("Users").Index("UX_Cms_Users_UserName").Exists())
            {
                Delete.UniqueConstraint("UX_Cms_Users_UserName").FromTable("Users").InSchema(SchemaName);
            }
            if (Schema.Schema(SchemaName).Table("Users").Index("UX_Cms_Users_Email").Exists())
            {
                Delete.UniqueConstraint("UX_Cms_Users_Email").FromTable("Users").InSchema(SchemaName);
            }

            // Other columns changed to nvarchchar
            Alter.Table("Users").InSchema(SchemaName).AlterColumn("UserName").AsString(UsersModuleConstants.UserNameMaxLength).NotNullable();
            Alter.Table("Users").InSchema(SchemaName).AlterColumn("Email").AsString(MaxLength.Email).NotNullable();

            // Re-create unique constraints
            Create
                .UniqueConstraint("UX_Cms_Users_UserName")
                .OnTable("Users").WithSchema(SchemaName)
                .Columns(new[] { "UserName", "DeletedOn" });

            Create
                .UniqueConstraint("UX_Cms_Users_Email")
                .OnTable("Users").WithSchema(SchemaName)
                .Columns(new[] { "Email", "DeletedOn" });
        }
    }
}