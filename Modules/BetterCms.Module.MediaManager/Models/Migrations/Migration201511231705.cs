using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201511231705)]
    public class Migration201511231705 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="201511231705"/> class.
        /// </summary>
        public Migration201511231705()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create.Column("IsMovedToTrash").OnTable("MediaFiles").InSchema(SchemaName).AsBoolean().NotNullable().WithDefaultValue(0);
            Create.Column("NextTryToMoveToTrash").OnTable("MediaFiles").InSchema(SchemaName).AsDateTime().Nullable();
        }        
    }
}