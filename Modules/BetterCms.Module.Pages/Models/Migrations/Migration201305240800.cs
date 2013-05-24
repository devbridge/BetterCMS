using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Creating dynamic layouts structure
    /// </summary>
    [Migration(201305240800)]
    public class Migration201305240800 : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201303050900"/> class.
        /// </summary>
        public Migration201305240800()
            : base(PagesModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            // Create dynamic layout contents table
            Create
                  .Table("DynamicLayoutContents")
                  .InSchema(SchemaName)
                  .WithColumn("Id").AsGuid().PrimaryKey()
                  .WithColumn("LayoutId").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_DynamicLayoutContents_Id_Contents_Id")
                .FromTable("DynamicLayoutContents").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Contents").InSchema(rootModuleSchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_DynamicLayoutContents_Id_Layouts_Id")
                .FromTable("DynamicLayoutContents").InSchema(SchemaName).ForeignColumn("LayoutId")
                .ToTable("Layouts").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
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