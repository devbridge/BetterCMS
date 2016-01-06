using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterCms.Module.Root.Models.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201308272154)]
    public class Migration201308272154: DefaultMigration
    {
        private readonly string rootModuleSchemaName;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308252344"/> class.
        /// </summary>
        public Migration201308272154()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new RootVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {            
            CreateMediaFileAccessRulesTable();
        }        

        private void CreateMediaFileAccessRulesTable()
        {
            if (Schema.Schema(SchemaName).Table("MediaFileAccess").Exists())
            {
                Delete.Index("IX_Cms_MediaFileAccess_MediaFileId").OnTable("MediaFileAccess").InSchema(SchemaName);
                Delete.ForeignKey("FK_Cms_MediaFileAccess_MediaFileId_Cms_MediaFile_Id").OnTable("MediaFileAccess").InSchema(SchemaName);
                Delete.Table("MediaFileAccess").InSchema(SchemaName);
            }

            Create
                .Table("MediaFileAccessRules")
                .InSchema(SchemaName)                
                .WithColumn("MediaFileId").AsGuid().NotNullable()
                .WithColumn("AccessRuleId").AsGuid().NotNullable();

            Create.PrimaryKey("PK_Cms_MediaFileAccessRules").OnTable("MediaFileAccessRules").WithSchema(SchemaName).Columns(new[] { "MediaFileId", "AccessRuleId" });

            Create
                .ForeignKey("FK_Cms_MediaFileAccessRules_MediaFileId_Cms_MediaFile_Id")
                .FromTable("MediaFileAccessRules").InSchema(SchemaName).ForeignColumn("MediaFileId")
                .ToTable("MediaFiles").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_MediaFileAccessRules_AccessRuleId_Cms_AccessRules_Id")
                .FromTable("MediaFileAccessRules").InSchema(SchemaName).ForeignColumn("AccessRuleId")
                .ToTable("AccessRules").InSchema(rootModuleSchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_MediaFileAccessRules_MediaFileId")
                .OnTable("MediaFileAccessRules").InSchema(SchemaName).OnColumn("MediaFileId");

            Create
                .Index("IX_Cms_MediaFileAccessRules_AccessRuleId")
                .OnTable("MediaFileAccessRules").InSchema(SchemaName).OnColumn("AccessRuleId");
        }
    }
}