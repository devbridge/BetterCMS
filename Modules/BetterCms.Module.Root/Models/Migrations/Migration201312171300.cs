using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201312171300)]
    public class Migration201312171300: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201312171300"/> class.
        /// </summary>
        public Migration201312171300()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Create new column
            Create
                .Column("LanguageGroupIdentifier")
                .OnTable("Pages").InSchema(SchemaName)
                .AsGuid().Nullable();

            // Drop old column, if such was created
            if (Schema.Schema(SchemaName).Table("Pages").Column("MainCulturePageId").Exists())
            {
                Delete
                    .ForeignKey("FK_Cms_Pages_MainCulturePageId_Cms_Pages_Id")
                    .OnTable("Pages").InSchema(SchemaName);

                Delete
                    .Column("MainCulturePageId")
                    .FromTable("Pages").InSchema(SchemaName);
            }
        }       
    }
}