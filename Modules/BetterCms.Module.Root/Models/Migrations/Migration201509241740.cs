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