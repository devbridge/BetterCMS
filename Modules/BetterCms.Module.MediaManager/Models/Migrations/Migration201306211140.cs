using System;
using System.Data.SqlTypes;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201306211140)]
    public class Migration201306211140: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306211140"/> class.
        /// </summary>
        public Migration201306211140()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Delete
                .Index("IX_Cms_MediaFolders_ParentId")
                .OnTable("MediaFolders").InSchema(SchemaName).OnColumn("ParentFolderId");

            Delete
                .ForeignKey("FK_Cms_MediaFolders_ParentId_MediaFolders_Id")
                .OnTable("MediaFolders").InSchema(SchemaName);

            Delete
                .Column("ParentFolderId")
                .FromTable("MediaFolders")
                .InSchema(SchemaName);
        }
    }
}