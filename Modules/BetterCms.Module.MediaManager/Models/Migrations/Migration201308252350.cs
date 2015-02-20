using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201308252350)]
    public class Migration201308252350: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308252350"/> class.
        /// </summary>
        public Migration201308252350()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create
                 .Table("MediaFileAccess")
                 .InSchema(SchemaName)
                 .WithBaseColumns()
                 .WithColumn("MediaFileId").AsGuid().NotNullable()
                 .WithColumn("RoleOrUser").AsString(MaxLength.Name).NotNullable()
                 .WithColumn("AccessLevel").AsInt32().NotNullable();

            Create
                .ForeignKey("FK_Cms_MediaFileAccess_MediaFileId_Cms_MediaFile_Id")
                .FromTable("MediaFileAccess").InSchema(SchemaName).ForeignColumn("MediaFileId")
                .ToTable("MediaFiles").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_MediaFileAccess_MediaFileId")
                .OnTable("MediaFileAccess").InSchema(SchemaName).OnColumn("MediaFileId");
        }
    }
}