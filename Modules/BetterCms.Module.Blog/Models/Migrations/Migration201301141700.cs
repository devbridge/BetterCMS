using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Blog.Models.Migrations
{
    [Migration(201301141700)]
    public class Migration201301141700 : DefaultMigration
    {
        /// <summary>
        /// The root schema name.
        /// </summary>
        private readonly string rootSchemaName;

        public Migration201301141700()
            : base(BlogModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            Create
               .Table("Options")
               .InSchema(SchemaName)
               .WithCmsBaseColumns()
               .WithColumn("DefaultLayoutId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_BlogOptions_Cms_Layouts")
                .FromTable("Options").InSchema(SchemaName).ForeignColumn("DefaultLayoutId")
                .ToTable("Layouts").InSchema(rootSchemaName).PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Cms_BlogOptions_Cms_Layouts").OnTable("Options").InSchema(SchemaName);
            Delete.Table("Options").InSchema(SchemaName);
        }     
    }
}