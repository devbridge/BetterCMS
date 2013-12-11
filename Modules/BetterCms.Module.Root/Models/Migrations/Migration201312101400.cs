using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201312101400)]
    public class Migration201312101400 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201312101400"/> class.
        /// </summary>
        public Migration201312101400()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateCulturesTable();
            AlterPagesTable();
        }       

        private void CreateCulturesTable()
        {
            Create
                .Table("Cultures").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Code").AsString(MaxLength.Name).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_Cultures_Name")
                .OnTable("Cultures").WithSchema(SchemaName)
                .Columns(new[] { "Name", "DeletedOn" });
            
            Create
                .UniqueConstraint("UX_Cms_Cultures_Code")
                .OnTable("Cultures").WithSchema(SchemaName)
                .Columns(new[] { "Code", "DeletedOn" });
        }
        
        private void AlterPagesTable()
        {
            Create
                .Column("CultureId")
                .OnTable("Pages").InSchema(SchemaName)
                .AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_Pages_Cms_Cultures")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("CultureId")
                .ToTable("Cultures").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Column("MainCulturePageId")
                .OnTable("Pages").InSchema(SchemaName)
                .AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_Pages_MainCulturePageId_Cms_Pages_Id")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("MainCulturePageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");
        }
    }
}