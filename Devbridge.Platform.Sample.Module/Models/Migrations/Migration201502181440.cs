using Devbridge.Platform.Core.DataAccess.DataContext.Migrations;
using Devbridge.Platform.Core.Models;

using FluentMigrator;

namespace Devbridge.Platform.Sample.Module.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201502181440)]
    public class Migration201302211227 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201302211227"/> class.
        /// </summary>
        public Migration201302211227()
            : base(SampleModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create
                 .Table("TestTable2").InSchema(SchemaName)
                 .WithBaseColumns()
                 .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                 .WithColumn("Description").AsString(MaxLength.Text).NotNullable();
        }
    }
}