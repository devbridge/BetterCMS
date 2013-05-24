using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// PageContents: added parentId.
    /// </summary>
    [Migration(201305240930)]
    public class Migration201305240930 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201305240930"/> class.
        /// </summary>
        public Migration201305240930()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            // Add parent id to page content
            Create
                .Column("ParentId")
                .OnTable("PageContents").InSchema(SchemaName)
                .AsGuid().Nullable();

            Create
               .ForeignKey("FK_Cms_PageContents_ParentId_PageContents_Id")
               .FromTable("PageContents").InSchema(SchemaName).ForeignColumn("ParentId")
               .ToTable("PageContents").InSchema(SchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Migrate down.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}