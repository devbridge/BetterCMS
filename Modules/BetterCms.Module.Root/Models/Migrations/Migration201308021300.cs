using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201308021300)]
    public class Migration201308021300: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308021300" /> class.
        /// </summary>
        public Migration201308021300()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Create table for layout options
            Create
                .Table("LayoutOptions")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("LayoutId").AsGuid().NotNullable()
                .WithColumn("Key").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("DefaultValue").AsString(MaxLength.Max).Nullable();

            Create
                .ForeignKey("FK_Cms_LayoutOptions_Type_Cms_OptionTypes_Id")
                .FromTable("LayoutOptions").InSchema(SchemaName).ForeignColumn("Type")
                .ToTable("OptionTypes").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_LayoutOptions_LayoutId_Cms_Layouts_Id")
                .FromTable("LayoutOptions").InSchema(SchemaName).ForeignColumn("LayoutId")
                .ToTable("Layouts").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .UniqueConstraint("UX_Cms_LayoutOptions_LayoutId_Key")
                .OnTable("LayoutOptions").WithSchema(SchemaName)
                .Columns(new[] { "LayoutId", "Key", "DeletedOn" });
        }
    }
}