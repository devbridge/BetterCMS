using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{    
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201510141900)]
    public class Migration201510141900 : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>

        public Migration201510141900()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Alter.Table("ContentOptionTranslations").InSchema(SchemaName).AlterColumn("Value").AsString(MaxLength.Max).Nullable();
            Alter.Table("ChildContentOptionTranslations").InSchema(SchemaName).AlterColumn("Value").AsString(MaxLength.Max).Nullable();
        }
    }
}