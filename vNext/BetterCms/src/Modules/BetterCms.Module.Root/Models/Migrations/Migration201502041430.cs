using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Categories: created parent category id and macro.
    /// </summary>
    [Migration(201502041430)]
    public class Migration201502041430 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201502041430"/> class.
        /// </summary>
        public Migration201502041430()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            if (Schema.Schema(SchemaName).Table("Categories").Index("UX_Cms_Categories_Name").Exists())
            {
                Delete.UniqueConstraint("UX_Cms_Categories_Name").FromTable("Categories").InSchema(SchemaName);
            }
        }
    }
}