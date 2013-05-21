using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201302011135)]
    public class Migration201302011135 : DefaultMigration
    {
        /// <summary>
        /// The media module schema name.
        /// </summary>
        private readonly string mediaModuleSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201302011135"/> class.
        /// </summary>
        public Migration201302011135()
            : base(UsersModuleDescriptor.ModuleName)
        {
            mediaModuleSchemaName = (new MediaManager.Models.Migrations.MediaManagerVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
            .ForeignKey("FK_Cms_Users_ImageId_Cms_MediaFiles_Id")
            .FromTable("Users").InSchema(SchemaName).ForeignColumn("ImageId")
            .ToTable("MediaFiles").InSchema(mediaModuleSchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}