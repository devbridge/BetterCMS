using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.LuceneSearch.Models.Migrations
{
    [Migration(201312190004)]
    public class InitialSetup : DefaultMigration
    {
        public InitialSetup()
            : base(LuceneSearchModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create
                .Table("IndexSources")
                .InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("SourceId").AsGuid().NotNullable()
                .WithColumn("Path").AsString(MaxLength.Url).NotNullable()
                .WithColumn("StartTime").AsDateTime().Nullable()
                .WithColumn("EndTime").AsDateTime().Nullable()
                .WithColumn("IsPublished").AsBoolean().NotNullable().WithDefaultValue(false);

            Create
                .Index("IX_Cms_IndexSources_SourceId")
                .OnTable("IndexSources")
                .InSchema(SchemaName)
                .OnColumn("SourceId");                
        }
    }
}
