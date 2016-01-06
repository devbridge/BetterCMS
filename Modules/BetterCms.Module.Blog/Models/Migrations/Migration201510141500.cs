using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Blog.Models.Migrations
{
    [Migration(201510141500)]
    public class Migration201510141500 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201510141500"/> class.
        /// </summary>
        public Migration201510141500()
            : base(BlogModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Alter
                .Column("Description")
                .OnTable("Authors")
                .InSchema(SchemaName)
                .AsString(MaxLength.Max).Nullable();
        }
    }
}