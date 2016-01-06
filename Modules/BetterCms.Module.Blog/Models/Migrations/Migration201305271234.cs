using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Blog.Models.Migrations
{
    [Migration(201305271234)]
    public class Migration201305271234: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201305271234"/> class.
        /// </summary>
        public Migration201305271234()
            : base(BlogModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
               .Column("ActivationDate")
               .OnTable("BlogPosts")
               .InSchema(SchemaName)
               .AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);

            Create
               .Column("ExpirationDate")
               .OnTable("BlogPosts")
               .InSchema(SchemaName)
               .AsDateTime().Nullable();
        }
    }
}