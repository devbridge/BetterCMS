using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    [Migration(201301091317)]
    public class Migration201301091317 : DefaultMigration
    {
        /// <summary>
        /// The root schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201301081200" /> class.
        /// </summary>
        public Migration201301091317()
            : base(PagesModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            CreateHtmlContentHistoryTable();
            CreateServerControlWidgetHistoryTable();
            CreateHtmlContentWidgetHistoryTable();
        }

        public override void Down()
        {
            RemoveHtmlContentWidgetHistoryTable();
            RemoveServerControlWidgetHistoryTable();
            RemoveHtmlContentHistoryTable();
        }
     
        private void CreateHtmlContentHistoryTable()
        {
            Create
               .Table("HtmlContentHistory")
               .InSchema(SchemaName)
               .WithColumn("Id").AsGuid().PrimaryKey()
               .WithColumn("ActivationDate").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
               .WithColumn("ExpirationDate").AsDateTime().Nullable()
               .WithColumn("CustomCss").AsString(MaxLength.Max).Nullable()
               .WithColumn("UseCustomCss").AsBoolean().NotNullable().WithDefaultValue(false)
               .WithColumn("CustomJs").AsString(MaxLength.Max).Nullable()
               .WithColumn("UseCustomJs").AsBoolean().NotNullable().WithDefaultValue(false)
               .WithColumn("Html").AsString(int.MaxValue).NotNullable();               
               
            Create
                .ForeignKey("FK_Cms_HtmlContentHistory_Id_ContentHistory_Id")
                .FromTable("HtmlContentHistory").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("ContentHistory").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemoveHtmlContentHistoryTable()
        {
            Delete.ForeignKey("FK_Cms_HtmlContentHistory_Id_ContentHistory_Id").OnTable("HtmlContentHistory").InSchema(SchemaName);
            Delete.Table("HtmlContentHistory").InSchema(SchemaName);
        }

        private void CreateServerControlWidgetHistoryTable()
        {
            Create
                .Table("ServerControlWidgetHistory")
                .InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey()                
                .WithColumn("Url").AsAnsiString(MaxLength.Url).NotNullable();

            Create
                .ForeignKey("FK_Cms_ServerControlWidgetHistory_Id_WidgetHistory_Id")
                .FromTable("ServerControlWidgetHistory").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("WidgetHistory").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }

        private void RemoveServerControlWidgetHistoryTable()
        {
            Delete.ForeignKey("FK_Cms_ServerControlWidgetHistory_Id_WidgetHistory_Id").OnTable("ServerControlWidgetHistory").InSchema(SchemaName);
            Delete.Table("ServerControlWidgetHistory").InSchema(SchemaName);
        }

        private void CreateHtmlContentWidgetHistoryTable()
        {
            Create
                .Table("HtmlContentWidgetHistory")
                .InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("CustomCss").AsString(MaxLength.Max).Nullable()
                .WithColumn("UseCustomCss").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CustomJs").AsString(MaxLength.Max).Nullable()
                .WithColumn("UseCustomJs").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("Html").AsString(MaxLength.Max).NotNullable()
                .WithColumn("UseHtml").AsBoolean().NotNullable().WithDefaultValue(false);

            Create
                .ForeignKey("FK_Cms_HtmlContentWidgetHistory_Id_WidgetHistory_Id")
                .FromTable("HtmlContentWidgetHistory").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("WidgetHistory").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }
     
        private void RemoveHtmlContentWidgetHistoryTable()
        {
            Delete.ForeignKey("FK_Cms_HtmlContentWidgetHistory_Id_WidgetHistory_Id").OnTable("HtmlContentWidgetHistory").InSchema(SchemaName);
            Delete.Table("HtmlContentWidgetHistory").InSchema(SchemaName);
        }       
    }
}