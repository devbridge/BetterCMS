using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201509281017)]
    public class Migration201509281017: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201509281017"/> class.
        /// </summary>
        public Migration201509281017()
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
                   Id = 21,
                   Name = "Multi-line Text"
               });
        }       
    }
}