using BetterCms.Core.DataAccess.DataContext.Migrations;
using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201212271611)]
    public class Migration201212271611 : DefaultMigration
    {
        public Migration201212271611()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Delete
                .Column("IsStored")
                .FromTable("MediaFiles").InSchema(SchemaName);

            Alter
                .Table("MediaFiles").InSchema(SchemaName)
                .AddColumn("IsUploaded").AsBoolean().NotNullable().WithDefaultValue(0)
                .AddColumn("IsCanceled").AsBoolean().NotNullable().WithDefaultValue(0);

            Alter
                .Table("MediaImages").InSchema(SchemaName)
                .AddColumn("IsOriginalUploaded").AsBoolean().NotNullable().WithDefaultValue(0)
                .AddColumn("IsThumbnailUploaded").AsBoolean().NotNullable().WithDefaultValue(0);
        }

        public override void Down()
        {
            Alter
                .Table("MediaFiles").InSchema(SchemaName)
                .AddColumn("IsStored").AsBoolean().NotNullable().WithDefaultValue(0);

            Delete
                .Column("IsUploaded")
                .FromTable("MediaFiles").InSchema(SchemaName);

            Delete
                 .Column("IsOriginalUploaded")
                 .FromTable("MediaImages").InSchema(SchemaName);

            Delete
               .Column("IsThumbnailUploaded")
               .FromTable("MediaImages").InSchema(SchemaName);
        }     
    }
}