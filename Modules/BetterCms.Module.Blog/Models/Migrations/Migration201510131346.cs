using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Blog.Models.Migrations
{
    [Migration(201510131346)]
    public class Migration201510131346 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201510131346"/> class.
        /// </summary>
        public Migration201510131346() : base(BlogModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Column("Description")
                .OnTable("Authors")
                .InSchema(SchemaName)
                .AsString().Nullable();
        }
    }
}