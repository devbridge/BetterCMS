using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201309040830)]
    public class Migration201309040830: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201309040830"/> class.
        /// </summary>
        public Migration201309040830()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Add column IsDeletable to option tables
            Alter
                .Table("LayoutOptions").InSchema(SchemaName)
                .AddColumn("IsDeletable")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(true);
            
            Alter
                .Table("ContentOptions").InSchema(SchemaName)
                .AddColumn("IsDeletable")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(true);
        }       
    }
}