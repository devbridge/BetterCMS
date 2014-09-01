using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Settings database structure create.
    /// </summary>
    [Migration(201408291545)]
    public class Migration201408291545 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201408291545"/> class.
        /// </summary>
        public Migration201408291545()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            //// Create table.
            //Create
            //    .Table("Settings")
            //    .InSchema(SchemaName)
            //    .WithCmsBaseColumns()
            //    .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
            //    .WithColumn("Value").AsString(MaxLength.Name)
            //    .WithColumn("ModuleId").AsGuid().NotNullable();
        }
    }
}