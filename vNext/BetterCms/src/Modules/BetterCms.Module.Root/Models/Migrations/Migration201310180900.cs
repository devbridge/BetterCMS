using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201310180900)]
    public class Migration201310180900: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310180900"/> class.
        /// </summary>
        public Migration201310180900()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Create custom option for adding CSS / JavaScript files
            Insert
               .IntoTable("OptionTypes").InSchema(SchemaName)
               .Row(new
               {
                   Id = 51,
                   Name = "JS Include URL"
               });

            Insert
               .IntoTable("OptionTypes").InSchema(SchemaName)
               .Row(new
               {
                   Id = 52,
                   Name = "CSS Include URL"
               });
        }       
    }
}