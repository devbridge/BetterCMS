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