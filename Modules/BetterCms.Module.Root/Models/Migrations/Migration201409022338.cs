using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Settings database structure create.
    /// </summary>
    [Migration(201409022338)]
    public class Migration201409022338 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201409022338"/> class.
        /// </summary>
        public Migration201409022338()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Create table.
            Create
                .Table("Setting")
                .InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Value").AsString(MaxLength.Name)
                .WithColumn("ModuleId").AsGuid().NotNullable();
        }
    }
}