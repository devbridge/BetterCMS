using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201309221100)]
    public class Migration201309221100: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201309221100"/> class.
        /// </summary>
        public Migration201309221100()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Add custom option to option types table
            Insert
                .IntoTable("OptionTypes").InSchema(SchemaName)
                .Row(new { Id = 99, Name = "Custom" });

            // Create custom options table
            Create
                .Table("CustomOptions")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("Title").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Identifier").AsString(MaxLength.Name).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_CustomOptions_Identifier")
                .OnTable("CustomOptions").WithSchema(SchemaName)
                .Columns(new[] { "Identifier", "DeletedOn" });

            // Add custom option references to all option tables
            Create
                .Column("CustomOptionId")
                .OnTable("ContentOptions").InSchema(SchemaName)
                .AsGuid().Nullable();

            Create
                .Column("CustomOptionId")
                .OnTable("PageContentOptions").InSchema(SchemaName)
                .AsGuid().Nullable();

            Create
                .Column("CustomOptionId")
                .OnTable("LayoutOptions").InSchema(SchemaName)
                .AsGuid().Nullable();
            
            Create
                .Column("CustomOptionId")
                .OnTable("PageOptions").InSchema(SchemaName)
                .AsGuid().Nullable();

            // Create foreign keys for custom options
            Create
                .ForeignKey("FK_Cms_ContentOptions_CustomOptionId_Cms_CustomOptions_Id")
                .FromTable("ContentOptions").InSchema(SchemaName).ForeignColumn("CustomOptionId")
                .ToTable("CustomOptions").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageContentOptions_CustomOptionId_Cms_CustomOptions_Id")
                .FromTable("PageContentOptions").InSchema(SchemaName).ForeignColumn("CustomOptionId")
                .ToTable("CustomOptions").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_LayoutOptions_CustomOptionId_Cms_CustomOptions_Id")
                .FromTable("LayoutOptions").InSchema(SchemaName).ForeignColumn("CustomOptionId")
                .ToTable("CustomOptions").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageOptions_CustomOptionId_Cms_CustomOptions_Id")
                .FromTable("PageOptions").InSchema(SchemaName).ForeignColumn("CustomOptionId")
                .ToTable("CustomOptions").InSchema(SchemaName).PrimaryColumn("Id");
        }       
    }
}