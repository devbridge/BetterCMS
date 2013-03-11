using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201303081430)]
    public class Migration201303081430 : DefaultMigration
    {
        public Migration201303081430()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Set IsUploaded as nullable
            Alter
                .Column("IsUploaded")
                .OnTable("MediaFiles").InSchema(SchemaName)
                .AsBoolean().Nullable();

            // Set IsOriginalUploaded as nullable
            Alter
                .Column("IsOriginalUploaded")
                .OnTable("MediaImages").InSchema(SchemaName)
                .AsBoolean().Nullable();

            // Set IsThumbnailUploaded as nullable
            Alter
                .Column("IsThumbnailUploaded")
                .OnTable("MediaImages").InSchema(SchemaName)
                .AsBoolean().Nullable();
        }

        public override void Down()
        {
            // Set IsUploaded as not nullable
            Alter
                .Column("IsUploaded")
                .OnTable("MediaFiles").InSchema(SchemaName)
                .AsBoolean().NotNullable().WithDefaultValue(false);

            // Set IsOriginalUploaded as not nullable
            Alter
                .Column("IsOriginalUploaded")
                .OnTable("MediaImages").InSchema(SchemaName)
                .AsBoolean().NotNullable().WithDefaultValue(false);

            // Set IsThumbnailUploaded as not nullable
            Alter
                .Column("IsThumbnailUploaded")
                .OnTable("MediaImages").InSchema(SchemaName)
                .AsBoolean().NotNullable().WithDefaultValue(false);
        }
    }
}