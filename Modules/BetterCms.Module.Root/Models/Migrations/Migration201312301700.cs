using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201312301700)]
    public class Migration201312301700: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201312301700"/> class.
        /// </summary>
        public Migration201312301700()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            if (Schema.Schema(SchemaName).Table("Pages").Column("CultureGroupIdentifier").Exists())
            {
                Rename
                    .Column("CultureGroupIdentifier")
                    .OnTable("Pages").InSchema(SchemaName)
                    .To("LanguageGroupIdentifier");
            }

            if (Schema.Schema(SchemaName).Table("Pages").Column("CultureId").Exists())
            {
                Rename
                    .Column("CultureId")
                    .OnTable("Pages").InSchema(SchemaName)
                    .To("LanguageId");
            }

            if (Schema.Schema(SchemaName).Table("Cultures").Exists())
            {
                Rename
                    .Table("Cultures").InSchema(SchemaName)
                    .To("Languages");
            }
        }       
    }
}