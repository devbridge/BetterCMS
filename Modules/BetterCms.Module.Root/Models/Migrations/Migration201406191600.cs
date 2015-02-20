using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Child contents database structure create.
    /// </summary>
    [Migration(201406191600)]
    public class Migration201406191600: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201406191600"/> class.
        /// </summary>
        public Migration201406191600()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateChildContentsTable();
            CreateChildContentOptionsTable();
        }

        /// <summary>
        /// Creates the child contents table.
        /// </summary>
        private void CreateChildContentsTable()
        {
            Create
                .Table("ChildContents").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("ParentContentId").AsGuid().NotNullable()
                .WithColumn("ChildContentId").AsGuid().NotNullable()
                .WithColumn("AssignmentIdentifier").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_ChildContents_ParentContentId_Contents_Id")
                .FromTable("ChildContents").InSchema(SchemaName).ForeignColumn("ParentContentId")
                .ToTable("Contents").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_ChildContents_ChildContentId_Contents_Id")
                .FromTable("ChildContents").InSchema(SchemaName).ForeignColumn("ChildContentId")
                .ToTable("Contents").InSchema(SchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the child content options table.
        /// </summary>
        private void CreateChildContentOptionsTable()
        {
            Create
                .Table("ChildContentOptions")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("ChildContentId").AsGuid().NotNullable()
                .WithColumn("Value").AsString(MaxLength.Max).Nullable()
                .WithColumn("Key").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("CustomOptionId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_ChildContentOptions_ChildContentId_Cms_ChildContents_Id")
                .FromTable("ChildContentOptions").InSchema(SchemaName).ForeignColumn("ChildContentId")
                .ToTable("ChildContents").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_ChildContentOptions_Type_Cms_OptionTypes_Id")
                .FromTable("ChildContentOptions").InSchema(SchemaName).ForeignColumn("Type")
                .ToTable("OptionTypes").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .UniqueConstraint("UX_Cms_ChildContentOptions_ChildContentId_Key")
                .OnTable("ChildContentOptions").WithSchema(SchemaName)
                .Columns(new[] { "ChildContentId", "Key", "DeletedOn" });

            Create
                .ForeignKey("FK_Cms_ChildContentOptions_CustomOptionId_Cms_CustomOptions_Id")
                .FromTable("ChildContentOptions").InSchema(SchemaName).ForeignColumn("CustomOptionId")
                .ToTable("CustomOptions").InSchema(SchemaName).PrimaryColumn("Id");
        }
    }
}