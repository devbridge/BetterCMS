using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201308252344)]
    public class Migration201308252344: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308252344"/> class.
        /// </summary>
        public Migration201308252344()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            if (Schema.Schema(SchemaName).Table("UserAccess").Exists())
            {
                Delete.Table("UserAccess").InSchema(SchemaName);
            }

            CreatePageAccessTable();
        }

        /// <summary>
        /// Creates the user access table.
        /// </summary>
        private void CreatePageAccessTable()
        {
            Create
                .Table("PageAccess")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("RoleOrUser").AsString(MaxLength.Name).NotNullable()
                .WithColumn("AccessLevel").AsInt32().NotNullable();

            Create
                .ForeignKey("FK_Cms_PageAccess_PageId_Cms_Page_Id")
                .FromTable("PageAccess").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_PageAccess_PageId")
                .OnTable("PageAccess").InSchema(SchemaName).OnColumn("PageId");
        }
    }
}