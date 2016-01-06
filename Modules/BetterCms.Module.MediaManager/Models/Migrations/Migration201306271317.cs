using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201306271317)]
    public class Migration201306271317: DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306271317"/> class.
        /// </summary>
        public Migration201306271317()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Table("MediaTags")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("MediaId").AsGuid().NotNullable()
                .WithColumn("TagId").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_MediaTags_Cms_Medias")
                .FromTable("MediaTags").InSchema(SchemaName).ForeignColumn("MediaId")
                .ToTable("Medias").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_MediaTags_Cms_Tags")
                .FromTable("MediaTags").InSchema(SchemaName).ForeignColumn("TagId")
                .ToTable("Tags").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }
    }
}