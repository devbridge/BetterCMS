using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    [Migration(201301210830)]
    public class Migration201301210830 : DefaultMigration
    {
        public Migration201301210830()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            AlterPagesTableUp();
        }

        public override void Down()
        {
            AlterPagesTableDown();
        }

        private void AlterPagesTableUp()
        {
            Alter
                .Table("Pages").InSchema(SchemaName)
                .AddColumn("CustomJS")
                .AsString(MaxLength.Max).Nullable();

            Delete
                .Column("UseCustomCSS")
                .FromTable("Pages").InSchema(SchemaName);
        }

        private void AlterPagesTableDown()
        {
            Delete
                .Column("CustomJS")
                .FromTable("Pages").InSchema(SchemaName);

            Alter
                .Table("Pages").InSchema(SchemaName)
                .AddColumn("UseCustomCSS")
                .AsBoolean().NotNullable().WithDefaultValue(true);
        }
    }
}