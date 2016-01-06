using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Categories: created parent category id and macro.
    /// </summary>
    [Migration(201501281145)]
    public class Migration201501281145 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201501281145"/> class.
        /// </summary>
        public Migration201501281145()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            if (!Schema.Schema(SchemaName).Table("CategoryTrees").Column("Macro").Exists())
            {
                Create
                    .Column("Macro")
                    .OnTable("CategoryTrees").InSchema(SchemaName)
                    .AsString(MaxLength.Text).Nullable();
            }
        }
    }
}