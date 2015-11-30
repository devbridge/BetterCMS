// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201510061845.cs" company="Devbridge Group LLC">
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
    [Migration(201510061845)]
    public class Migration201510061845 : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>

        public Migration201510061845() : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateChildContentOptionsTranslationsTable();
        }

        private void CreateChildContentOptionsTranslationsTable()
        {
            Create
                .Table("ChildContentOptionTranslations")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("ChildContentOptionId").AsGuid().NotNullable()
                .WithColumn("LanguageId").AsGuid().NotNullable()
                .WithColumn("Value").AsString(MaxLength.Max).NotNullable();

            Create
                .ForeignKey("FK_Cms_ChildContentOptionTranslations_ChildContentOptionId_Cms_ChildContentOptions_Id")
                .FromTable("ChildContentOptionTranslations").InSchema(SchemaName).ForeignColumn("ChildContentOptionId")
                .ToTable("ChildContentOptions").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_ChildContentOptionTranslations_LanguageId_Cms_Languages_Id")
                .FromTable("ChildContentOptionTranslations").InSchema(SchemaName).ForeignColumn("LanguageId")
                .ToTable("Languages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .UniqueConstraint("UX_Cms_ChildContentOptionTranslations_ChildContentOption_Language")
                .OnTable("ChildContentOptionTranslations")
                .WithSchema(SchemaName)
                .Columns(new []{"ChildContentOptionId", "LanguageId", "DeletedOn"});
        }
    }
}