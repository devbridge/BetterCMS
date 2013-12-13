using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.LuceneSearch.Models.Migrations
{
    [Migration(201312130922)]
    public class InitialSetup : DefaultMigration
    {
        public InitialSetup()
            : base(LuceneSearchModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create.Table("IndexedPages").InSchema(SchemaName).WithCmsBaseColumns()
                .WithColumn("Path").AsString(MaxLength.Url).NotNullable()
                .WithColumn("StartTime").AsDateTime().Nullable()
                .WithColumn("EndTime").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("IndexedPages").InSchema(SchemaName);
        }
    }
}
