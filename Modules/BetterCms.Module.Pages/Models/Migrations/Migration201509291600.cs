using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201509291600)]
    public class Migration201509291600 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201509291600"/> class.
        /// </summary>
        public Migration201509291600()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Alter.Table("HtmlContents").InSchema(SchemaName)
                .AddColumn("OriginalText").AsString(MaxLength.Max).Nullable()
                .AddColumn("ContentTextMode").AsInt32().NotNullable().WithDefaultValue("1");

            Create
                .Table("ContentTextModes")
                .InSchema(SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_ContentTextModes_Name")
                .OnTable("ContentTextModes").WithSchema(SchemaName)
                .Column("Name");

            Insert
                .IntoTable("ContentTextModes")
                .InSchema(SchemaName)
                .Row(new
                {
                    Id = 1,
                    Name = "Html"
                })
                .Row(new
                {
                    Id = 2,
                    Name = "Markdown"
                })
                .Row(new
                {
                    Id = 3,
                    Name = "SimpleText"
                });

            Create
                .ForeignKey("FK_Cms_HtmlContents_ContentTextMode_ContentTextModes_Id")
                .FromTable("HtmlContents").InSchema(SchemaName).ForeignColumn("ContentTextMode")
                .ToTable("ContentTextModes").InSchema(SchemaName).PrimaryColumn("Id");
        }
    }
}