using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201312111700)]
    public class Migration201312111700: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201312111700"/> class.
        /// </summary>
        public Migration201312111700()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create
                .Column("LanguageId")
                .OnTable("Pages").InSchema(SchemaName)
                .AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_Pages_Cms_Languages")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("LanguageId")
                .ToTable("Languages").InSchema(SchemaName).PrimaryColumn("Id");
        }       
    }
}