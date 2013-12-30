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
            CreateLanguagesTable();
        }

        private void CreateLanguagesTable()
        {
            Create
                .Table("Languages").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Code").AsString(MaxLength.Name).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_Languages_Name")
                .OnTable("Languages").WithSchema(SchemaName)
                .Columns(new[] { "Name", "DeletedOn" });
            
            Create
                .UniqueConstraint("UX_Cms_Languages_Code")
                .OnTable("Languages").WithSchema(SchemaName)
                .Columns(new[] { "Code", "DeletedOn" });
        }
    }
}