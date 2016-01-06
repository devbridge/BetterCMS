using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Migration for IsInSitemap.
    /// </summary>
    [Migration(201306281001)]
    public class Migration201306281001: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306281001"/> class.
        /// </summary>
        public Migration201306281001()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            if (!Schema.Schema(SchemaName).Table("Medias").Column("Description").Exists())
            {
                Alter.Table("Medias").InSchema(SchemaName).AddColumn("Description").AsString(MaxLength.Text).Nullable();
            }
        }
    }
}