using System;
using System.Data.SqlTypes;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201306201215)]
    public class Migration201306201215: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306201215"/> class.
        /// </summary>
        public Migration201306201215()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Column("PublishedOn")
                .OnTable("Medias").InSchema(SchemaName)
                .AsDateTime().Nullable();

            Update
                .Table("Medias").InSchema(SchemaName)
                .Set(new { PublishedOn = (DateTime)SqlDateTime.MinValue })
                .AllRows();

            Alter
               .Column("PublishedOn")
               .OnTable("Medias").InSchema(SchemaName)
               .AsDateTime().NotNullable();
        }
    }
}