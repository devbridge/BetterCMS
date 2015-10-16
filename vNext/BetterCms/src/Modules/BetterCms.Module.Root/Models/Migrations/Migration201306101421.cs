using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201306101421)]
    public class Migration201306101421: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306101421"/> class.
        /// </summary>
        public Migration201306101421() : base(RootModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            // Unpublish hidden pages.
            Update
                .Table("Pages")
                .InSchema(SchemaName)
                .Set(new { Status = 4 })
                .Where(new { Status = 3, IsPublic = 0 });

            Delete
                .Column("IsPublic")
                .FromTable("Pages")
                .InSchema(SchemaName);
        }
    }
}