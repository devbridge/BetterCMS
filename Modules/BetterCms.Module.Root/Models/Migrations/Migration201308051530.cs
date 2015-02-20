using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201308051530)]
    public class Migration201308051530: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308051530" /> class.
        /// </summary>
        public Migration201308051530()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Create table for page options
            Create
                .Table("PageOptions")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("Value").AsString(MaxLength.Max).Nullable()
                .WithColumn("Key").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Type").AsInt32().NotNullable();

            Create
                .ForeignKey("FK_Cms_PageOptions_PageId_Cms_Pages_Id")
                .FromTable("PageOptions").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageOptions_Type_Cms_OptionTypes_Id")
                .FromTable("PageOptions").InSchema(SchemaName).ForeignColumn("Type")
                .ToTable("OptionTypes").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .UniqueConstraint("UX_Cms_PageOptions_PageId_Key")
                .OnTable("PageOptions").WithSchema(SchemaName)
                .Columns(new[] { "PageId", "Key", "DeletedOn" });
        }
    }
}