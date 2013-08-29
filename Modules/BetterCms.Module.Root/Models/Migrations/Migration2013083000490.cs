using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(2013083000490)]
    public class Migration2013083000490 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration2013083000490"/> class.
        /// </summary>
        public Migration2013083000490()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter.Table("AccessRules").InSchema(SchemaName).AlterColumn("Identity").AsString(MaxLength.Max).NotNullable();
        }       
    }
}