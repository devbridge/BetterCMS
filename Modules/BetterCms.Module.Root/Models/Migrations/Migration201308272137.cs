using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201308272137)]
    public class Migration201308272137: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308252344"/> class.
        /// </summary>
        public Migration201308272137()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateAccessRulesTable();
            CreatePageAccessRulesTable();
        }

        private void CreateAccessRulesTable()
        {
            Create
                .Table("AccessRules")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("Identity").AsString(MaxLength.Name).NotNullable()
                .WithColumn("AccessLevel").AsInt32().NotNullable();
        }

        private void CreatePageAccessRulesTable()
        {
            if (Schema.Schema(SchemaName).Table("PageAccess").Exists())
            {
                Delete.Index("IX_Cms_PageAccess_PageId").OnTable("PageAccess").InSchema(SchemaName);
                Delete.ForeignKey("FK_Cms_PageAccess_PageId_Cms_Page_Id").OnTable("PageAccess").InSchema(SchemaName);
                Delete.Table("PageAccess").InSchema(SchemaName);
            }

            Create
                .Table("PageAccessRules")
                .InSchema(SchemaName)
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("AccessRuleId").AsGuid().NotNullable();

            Create.PrimaryKey("PK_Cms_PageAccessRules").OnTable("PageAccessRules").WithSchema(SchemaName).Columns(new[] { "PageId", "AccessRuleId" });

            Create
                .ForeignKey("FK_Cms_PageAccessRules_PageId_Cms_Page_Id")
                .FromTable("PageAccessRules").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageAccessRules_AccessRuleId_Cms_AccessRules_Id")
                .FromTable("PageAccessRules").InSchema(SchemaName).ForeignColumn("AccessRuleId")
                .ToTable("AccessRules").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_PageAccessRules_PageId")
                .OnTable("PageAccessRules").InSchema(SchemaName).OnColumn("PageId");

            Create
                .Index("IX_Cms_PageAccessRules_AccessRuleId")
                .OnTable("PageAccessRules").InSchema(SchemaName).OnColumn("AccessRuleId");
        }
    }
}