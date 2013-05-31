using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Creating dynamic html layouts contents structure
    /// </summary>
    [Migration(201305311100)]
    public class Migration201305311100 : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201303050900"/> class.
        /// </summary>
        public Migration201305311100()
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
                  .Table("DynamicHtmlLayoutContents")
                  .InSchema(SchemaName)
                  .WithColumn("Id").AsGuid().PrimaryKey()
                  .WithColumn("Html").AsString(MaxLength.Max).NotNullable();

            Create
                .ForeignKey("FK_Cms_DynamicHtmlLayoutContents_Id_Contents_Id")
                .FromTable("DynamicHtmlLayoutContents").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Contents").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
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