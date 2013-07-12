using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// module database structure update.
    /// </summary>
    [Migration(201305230800)]
    public class Migration201305230800 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201305230800"/> class.
        /// </summary>
        public Migration201305230800() : base(RootModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            // Create table
            Create
                .Table("ModulesContentVersions")
                .InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("ModuleName").AsString(MaxLength.Name).NotNullable()
                .WithColumn("ContentVersion").AsInt64().NotNullable();
        }
    }
}