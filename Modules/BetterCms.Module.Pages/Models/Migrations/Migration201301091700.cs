using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// This migration script removes ImageUrl from pages table and adds reference to MediaImages table
    /// </summary>
    [Migration(201301091700)]
    public class Migration201301091700 : DefaultMigration
    {
        /// <summary>
        /// The media manager schema name.
        /// </summary>
        private readonly string mediaManagerSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201301091700" /> class.
        /// </summary>
        public Migration201301091700()
            : base(PagesModuleDescriptor.ModuleName)
        {
            mediaManagerSchemaName = (new MediaManager.Models.Migrations.MediaManagerVersionTableMetaData()).SchemaName;
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
                .Column("ImageUrl")
                .FromTable("Pages")
                .InSchema(SchemaName);

            Alter
                 .Table("Pages").InSchema(SchemaName)
                 .AddColumn("ImageId")
                 .AsGuid()
                 .Nullable();

            Create.ForeignKey("FK_Cms_Pages_ImageId_MediaImages_Id")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("ImageId")
                .ToTable("MediaImages").InSchema(mediaManagerSchemaName).PrimaryColumn("Id");
        }

        private void AlterPagesTableDown()
        {
            Alter
                 .Table("Pages").InSchema(SchemaName)
                 .AddColumn("ImageUrl")
                 .AsString(MaxLength.Url)
                 .Nullable();

            Delete.ForeignKey("FK_Cms_Pages_ImageId_MediaImages_Id")
                .OnTable("Pages").InSchema(SchemaName);

            Delete
                .Column("ImageId")
                .FromTable("Pages")
                .InSchema(SchemaName);
        }
    }
}