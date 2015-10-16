using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// module database structure update.
    /// </summary>
    [Migration(201303261100)]
    public class Migration201303261100: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201303261100"/> class.
        /// </summary>
        public Migration201303261100()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            // Set non valid statuses as unpublished
            Update
                .Table("Pages").InSchema(SchemaName)
                .Set(new { Status = 4 })
                .Where(new { Status = 0 });

            // Create table
            Create
                .Table("PageStatuses")
                .InSchema(SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            // Create Uq
            Create
                .UniqueConstraint("UX_Cms_PageStatuses_Name")
                .OnTable("PageStatuses").WithSchema(SchemaName)
                .Column("Name");

            // Insert page statuses
            Insert
                .IntoTable("PageStatuses")
                .InSchema(SchemaName)
                .Row(new
                {
                    Id = 1,
                    Name = "Preview"
                })
                .Row(new
                {
                    Id = 2,
                    Name = "Draft"
                })
                .Row(new
                {
                    Id = 3,
                    Name = "Published"
                })
                .Row(new
                {
                    Id = 4,
                    Name = "Unpublished"
                });

            // Create FK
            Create
                .ForeignKey("FK_Cms_RootPages_Cms_PageStatuses")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("Status")
                .ToTable("PageStatuses").InSchema(SchemaName).PrimaryColumn("Id");
        }
    }
}