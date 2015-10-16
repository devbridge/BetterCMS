using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201308211600)]
    public class Migration201308211600: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308211600"/> class.
        /// </summary>
        public Migration201308211600()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Delete.Table("Users").InSchema(SchemaName);
        }
    }
}