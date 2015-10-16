using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201308300049)]
    public class Migration201308300049: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308300049"/> class.
        /// </summary>
        public Migration201308300049()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter.Table("AccessRules").InSchema(SchemaName).AlterColumn("Identity").AsString(MaxLength.Max).NotNullable();
        }       
    }
}