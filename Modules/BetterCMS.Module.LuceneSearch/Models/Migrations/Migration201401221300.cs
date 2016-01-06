using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.LuceneSearch.Models.Migrations
{
    [Migration(201401221300)]
    public class Migration201401221300: DefaultMigration
    {
        public Migration201401221300()
            : base(LuceneSearchModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter
                .Table("IndexSources").InSchema(SchemaName)
                .AddColumn("NextRetryTime").AsDateTime().Nullable();

            Alter
                .Table("IndexSources").InSchema(SchemaName)
                .AddColumn("FailedCount").AsInt32().NotNullable().WithDefaultValue(0);
        }
    }
}
