using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Vimeo.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201307111459)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(VimeoModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Table("Videos").InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("VideoId").AsString(MaxLength.Name).Nullable()
                .WithColumn("ThumbnailUrl").AsString(MaxLength.Url).Nullable()
                .WithColumn("AuthorName").AsString(MaxLength.Name).Nullable()
                .WithColumn("AuthorUrl").AsString(MaxLength.Url).Nullable();
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