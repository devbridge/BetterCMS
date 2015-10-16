using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201308060930)]
    public class Migration201308060930: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308060930" /> class.
        /// </summary>
        public Migration201308060930()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Insert new option types to option types table
            Insert
                .IntoTable("OptionTypes")
                .InSchema(SchemaName)
                .Row(new { Id = 2, Name = "Integer" });
            
            Insert
                .IntoTable("OptionTypes")
                .InSchema(SchemaName)
                .Row(new { Id = 3, Name = "Float" });
            
            Insert
                .IntoTable("OptionTypes")
                .InSchema(SchemaName)
                .Row(new { Id = 4, Name = "DateTime" });
            
            Insert
                .IntoTable("OptionTypes")
                .InSchema(SchemaName)
                .Row(new { Id = 5, Name = "Boolean" });
        }
    }
}