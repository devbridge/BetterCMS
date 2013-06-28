using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Migration for IsInSitemap.
    /// </summary>
    [Migration(2013062810010)]
    public class Migration2013062810010 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration2013062810010"/> class.
        /// </summary>
        public Migration2013062810010()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            Alter
                .Table("Medias").InSchema(SchemaName)
                .AddColumn("Description").AsString(MaxLength.Text).Nullable();
        }

        /// <summary>
        /// Migrate down.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}