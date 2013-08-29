using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(2013082921250)]
    public class Migration2013082921250 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration2013082921250"/> class.
        /// </summary>
        public Migration2013082921250()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter.Table("AccessRules").InSchema(SchemaName).AddColumn("IsForRole").AsBoolean().NotNullable().WithDefaultValue(true);
        }       
    }
}