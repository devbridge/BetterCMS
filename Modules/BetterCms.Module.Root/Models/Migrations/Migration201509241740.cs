// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201509241740.cs" company="Devbridge Group LLC">
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
    /// Module database structure update.
    /// </summary>
    [Migration(201509241740)]
    public class Migration201509241740 : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>

        public Migration201509241740() : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateContentOptionsTranslationsTable();
        }

        private void CreateContentOptionsTranslationsTable()
        {
            Create
                .Table("ContentOptionTranslations")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("ContentOptionId").AsGuid().NotNullable()
                .WithColumn("LanguageId").AsGuid().NotNullable()
                .WithColumn("Value").AsString(MaxLength.Max).NotNullable();

            Create
                .ForeignKey("FK_Cms_ContentOptionTranslations_ContentOptionId_Cms_ContentOptions_Id")
                .FromTable("ContentOptionTranslations").InSchema(SchemaName).ForeignColumn("ContentOptionId")
                .ToTable("ContentOptions").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_ContentOptionTranslations_LanguageId_Cms_Languages_Id")
                .FromTable("ContentOptionTranslations").InSchema(SchemaName).ForeignColumn("LanguageId")
                .ToTable("Languages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .UniqueConstraint("UX_Cms_ContentOptionTranslations_ContentOption_Language")
                .OnTable("ContentOptionTranslations")
                .WithSchema(SchemaName)
                .Columns(new []{"ContentOptionId", "LanguageId", "DeletedOn"});
        }
    }
}