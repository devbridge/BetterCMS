using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterCms.Module.Root.Models.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Blog.Models.Migrations
{
    [Migration(201312051034)]
    public class Migration201312051034: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201312051034"/> class.
        /// </summary>
        public Migration201312051034()
            : base(BlogModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Column("DefaultMasterPageId")
                .OnTable("Options")
                .InSchema(SchemaName)
                .AsGuid().Nullable();
            
            Create
                .ForeignKey("FK_Cms_BlogOptions_Cms_Pages")
                .FromTable("Options").InSchema(SchemaName).ForeignColumn("DefaultMasterPageId")
                .ToTable("Pages").InSchema((new RootVersionTableMetaData()).SchemaName).PrimaryColumn("Id");
        }
    }
}