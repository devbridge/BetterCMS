using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// This migration script add  /removes columns from pages table
    /// </summary>
    [Migration(201301081200)]
    public class Migration201301081200 : DefaultMigration
    {
        /// <summary>
        /// The root schema name.
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201301081200" /> class.
        /// </summary>
        public Migration201301081200()
            : base(PagesModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Alters pages table.
        /// </summary>
        public override void Up()
        {
            AlterPagesTableUp();
        }

        /// <summary>
        /// Alters pages table.
        /// </summary>
        public override void Down()
        {
            AlterPagesTableDown();
        }

        private void AlterPagesTableUp()
        {
            Delete
                .Column("ShowTitle")
                .FromTable("Pages")
                .InSchema(SchemaName);

            Alter
                 .Table("Pages").InSchema(SchemaName)
                 .AddColumn("CategoryId")
                 .AsGuid()
                 .Nullable();

            Create.ForeignKey("FK_Cms_Pages_CategoryId_Categories_Id")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(rootSchemaName).PrimaryColumn("Id");
        }

        private void AlterPagesTableDown()
        {
            Alter
                 .Table("Pages").InSchema(SchemaName)
                 .AddColumn("ShowTitle")
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(true);

            Delete.ForeignKey("FK_Cms_Pages_CategoryId_Categories_Id")
                .OnTable("Pages").InSchema(SchemaName);

            Delete
                .Column("CategoryId")
                .FromTable("Pages")
                .InSchema(SchemaName);
        }
    }
}