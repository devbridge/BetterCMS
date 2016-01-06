using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Page content: created page content id.
    /// </summary>
    [Migration(201408131530)]
    public class Migration201408131530: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201408131530"/> class.
        /// </summary>
        public Migration201408131530()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            if (!Schema.Schema(SchemaName).Table("PageContents").Column("ParentPageContentId").Exists())
            {
                Create
                    .Column("ParentPageContentId")
                    .OnTable("PageContents").InSchema(SchemaName)
                    .AsGuid().Nullable();

                Create
                    .ForeignKey("Fk__PageContents_ParentPageContentId__ParentPageContentId_Id")
                    .FromTable("PageContents").InSchema(SchemaName).ForeignColumn("ParentPageContentId")
                    .ToTable("PageContents").InSchema(SchemaName).PrimaryColumn("Id");
            }
        }
    }
}