using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Categories: created parent category id and macro.
    /// </summary>
    [Migration(201502101035)]
    public class Migration201502101035 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201502101035"/> class.
        /// </summary>
        public Migration201502101035()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            if (!Schema.Schema(SchemaName).Table("CategorizableItems").Exists())
            {
                Create
                .Table("CategorizableItems").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

                Create
                    .UniqueConstraint("UX_Cms_CategorizableItems_Name")
                    .OnTable("CategorizableItems").WithSchema(SchemaName)
                    .Columns(new[] { "Name", "DeletedOn" });
            }

            if (!Schema.Schema(SchemaName).Table("CategoryTreeCategorizableItems").Exists())
            {
                Create
               .Table("CategoryTreeCategorizableItems")
               .InSchema(SchemaName)
               .WithBaseColumns()
               .WithColumn("CategoryTreeId").AsGuid().NotNullable()
               .WithColumn("CategorizableItemId").AsGuid().NotNullable();

                Create
                    .ForeignKey("FK_Cms_CategoryTreeCategorizableItems_Cms_CategoryTrees")
                    .FromTable("CategoryTreeCategorizableItems").InSchema(SchemaName).ForeignColumn("CategoryTreeId")
                    .ToTable("CategoryTrees").InSchema(SchemaName).PrimaryColumn("Id");

                Create
                    .ForeignKey("FK_Cms_CategoryTreeCategorizableItems_Cms_CategorizableItems")
                    .FromTable("CategoryTreeCategorizableItems").InSchema(SchemaName).ForeignColumn("CategorizableItemId")
                    .ToTable("CategorizableItems").InSchema(SchemaName).PrimaryColumn("Id");
            }
        }
    }
}