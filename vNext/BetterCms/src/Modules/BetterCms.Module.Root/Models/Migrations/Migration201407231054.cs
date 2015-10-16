using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Child contents database structure create.
    /// </summary>
    [Migration(201407231054)]
    public class Migration201407231054: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201407231054"/> class.
        /// </summary>
        public Migration201407231054()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Create table.
            Create
                .Table("ProtocolForcingTypes")
                .InSchema(SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            // Create Uq.
            Create
                .UniqueConstraint("UX_Cms_ProtocolForcingTypes_Name")
                .OnTable("ProtocolForcingTypes").WithSchema(SchemaName)
                .Column("Name");

            // Insert page access protocols.
            Insert
                .IntoTable("ProtocolForcingTypes")
                .InSchema(SchemaName)
                .Row(new
                {
                    Id = 0,
                    Name = "None"
                })
                .Row(new
                {
                    Id = 1,
                    Name = "ForceHttp"
                })
                .Row(new
                {
                    Id = 2,
                    Name = "ForceHttps"
                });

            // Create new column.
            Create
                .Column("ForceAccessProtocol")
                .OnTable("Pages").InSchema(SchemaName)
                .AsInt32()
                .WithDefaultValue(0)
                .NotNullable();

            // Create FK.
            Create
                .ForeignKey("FK_Cms_RootPages_Cms_ProtocolForcingTypes")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("ForceAccessProtocol")
                .ToTable("ProtocolForcingTypes").InSchema(SchemaName).PrimaryColumn("Id");
        }
    }
}