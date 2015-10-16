using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.DataContracts.Enums;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Fix for issue: https://github.com/devbridge/BetterCMS/issues/840
    /// </summary>
    [Migration(201404171400)]
    public class Migration201404171400: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201404171400"/> class.
        /// </summary>
        public Migration201404171400()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Update.Table("Pages")
                .InSchema(SchemaName)
                .Set(new { Status = (int)PageStatus.Published })
                .Where(new { Status = (int)PageStatus.Unpublished, IsDeleted = 0, IsMasterPage = 1 });
        }       
    }
}