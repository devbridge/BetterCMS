using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// This migration script refactords Authors table - adds/removed fields
    /// </summary>
    [Migration(201301140930)]
    public class Migration201301140930 : DefaultMigration
    {
        private const string authorsTableName = "Authors";

        /// <summary>
        /// The media manager schema name.
        /// </summary>
        private readonly string mediaManagerSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201301140930" /> class.
        /// </summary>
        public Migration201301140930()
            : base(PagesModuleDescriptor.ModuleName)
        {
            mediaManagerSchemaName = (new MediaManager.Models.Migrations.MediaManagerVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Alters pages table.
        /// </summary>
        public override void Up()
        {
            AlterAuthorsTableUp();
        }

        /// <summary>
        /// Alters pages table.
        /// </summary>
        public override void Down()
        {
            AlterAuthorsTableDown();
        }

        private void AlterAuthorsTableUp()
        {
            var columnsToDelete = new[] 
                                      {
                                          "FirstName", "LastName", "Title", "Email", "Twitter", "ProfileImageUrl", 
                                          "ProfileThumbnailUrl", "ShortDescription","LongDescription"
                                      };
            foreach (var column in columnsToDelete)
            {
                Delete
                    .Column(column)
                    .FromTable(authorsTableName)
                    .InSchema(SchemaName);
            }

            Rename
                .Column("DisplayName")
                .OnTable(authorsTableName)
                .InSchema(SchemaName)
                .To("Name");

            Alter
                .Table(authorsTableName).InSchema(SchemaName)
                .AddColumn("ImageId")
                .AsGuid()
                .Nullable();

            Create.ForeignKey("FK_Cms_Authors_ImageId_MediaImages_Id")
                .FromTable(authorsTableName).InSchema(SchemaName).ForeignColumn("ImageId")
                .ToTable("MediaImages").InSchema(mediaManagerSchemaName).PrimaryColumn("Id");
        }

        private void AlterAuthorsTableDown()
        {
            Delete.ForeignKey("FK_Cms_Authors_ImageId_MediaImages_Id")
                .OnTable(authorsTableName).InSchema(SchemaName);

            Delete
                .Column("ImageId")
                .FromTable(authorsTableName)
                .InSchema(SchemaName);

            Rename
                .Column("Name")
                .OnTable(authorsTableName)
                .InSchema(SchemaName)
                .To("DisplayName");

            // Restore dropped columns
            Alter.Table(authorsTableName).InSchema(SchemaName).AddColumn("FirstName").AsString(MaxLength.Name).NotNullable().WithDefaultValue(string.Empty);
            Alter.Table(authorsTableName).InSchema(SchemaName).AddColumn("LastName").AsString(MaxLength.Name).NotNullable().WithDefaultValue(string.Empty);
            Alter.Table(authorsTableName).InSchema(SchemaName).AddColumn("Title").AsString(MaxLength.Name).Nullable();
            Alter.Table(authorsTableName).InSchema(SchemaName).AddColumn("Email").AsString(MaxLength.Email).Nullable();
            Alter.Table(authorsTableName).InSchema(SchemaName).AddColumn("Twitter").AsString(MaxLength.Name).Nullable();
            Alter.Table(authorsTableName).InSchema(SchemaName).AddColumn("ProfileImageUrl").AsString(MaxLength.Url).Nullable();
            Alter.Table(authorsTableName).InSchema(SchemaName).AddColumn("ProfileThumbnailUrl").AsString(MaxLength.Url).Nullable();
            Alter.Table(authorsTableName).InSchema(SchemaName).AddColumn("ShortDescription").AsString(MaxLength.Text).Nullable();
            Alter.Table(authorsTableName).InSchema(SchemaName).AddColumn("LongDescription").AsString(MaxLength.Max).Nullable();
        }
    }
}