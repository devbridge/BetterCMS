using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201301101840)]
    public class Migration201301101840 : DefaultMigration
    {
        public Migration201301101840()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create
                .Table("ContentStatuses")
                .InSchema(SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_ContentStatuses_Name")
                .OnTable("ContentStatuses").WithSchema(SchemaName)
                .Column("Name");

            Insert
                .IntoTable("ContentStatuses")
                .InSchema(SchemaName)
                .Row(new
                        {
                            Id = 1,
                            Name = "Preview"
                        })
                .Row(new
                        {
                            Id = 2,
                           Name = "Draft"
                        })
                .Row(new
                        {
                            Id = 3,
                           Name = "Published"
                        })
                .Row(new
                        {
                            Id = 4,
                           Name = "Archived"
                        });
            
            /* PageContents table. */
            Alter
                 .Table("PageContents").InSchema(SchemaName)
                 .AddColumn("Status").AsInt32().NotNullable().WithDefaultValue(3);

            Create
                .ForeignKey("FK_Cms_PageContents_Status_ContentStatuses_Id")
                .FromTable("PageContents").InSchema(SchemaName).ForeignColumn("Status")
                .ToTable("ContentStatuses").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_PageContents_Status")
                .OnTable("PageContents").InSchema(SchemaName)
                .OnColumn("Status");

            /* ContentHistory table. */
            Alter
              .Table("PageContentHistory").InSchema(SchemaName)
              .AddColumn("Status").AsInt32().NotNullable().WithDefaultValue(3);

            Create
                .ForeignKey("FK_Cms_PageContentHistory_Status_ContentStatuses_Id")
                .FromTable("PageContentHistory").InSchema(SchemaName).ForeignColumn("Status")
                .ToTable("ContentStatuses").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_PageContentHistory_Status")
                .OnTable("PageContentHistory").InSchema(SchemaName)
                .OnColumn("Status");
        }

        public override void Down()
        {
            /* Contents table. */
            Delete.Index("IX_Cms_PageContents_Status").OnTable("PageContents").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageContents_Status_ContentStatuses_Id").OnTable("PageContents").InSchema(SchemaName);
            Delete.Column("Status").FromTable("PageContents").InSchema(SchemaName);
            
            /* ContentHistory table. */
            Delete.Index("IX_Cms_PageContentHistory_Status").OnTable("PageContentHistory").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageContentHistory_Status_ContentStatuses_Id").OnTable("PageContentHistory").InSchema(SchemaName);
            Delete.Column("Status").FromTable("PageContentHistory").InSchema(SchemaName);

            Delete.UniqueConstraint("UX_Cms_ContentStatuses_Name").FromTable("ContentStatuses").InSchema(SchemaName);
            Delete.Table("ContentStatuses").InSchema(SchemaName);
        } 
    }
}