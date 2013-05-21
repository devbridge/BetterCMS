using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Newsletter.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201304221200)]
    public class InitialSetup : DefaultMigration
    {
        private static readonly Guid widgetId = new Guid("C7F20821-E9FF-43AE-B8A0-63858447A72B");
        private static readonly Guid option1Id = new Guid("84E5C6DB-2801-4294-87AA-8ACA295EB792");
        private static readonly Guid option2Id = new Guid("44C9DAC8-1924-4F9C-9829-68E3A4B0D384");
        private static readonly Guid option3Id = new Guid("CD2DDAE1-F128-4AF2-B141-954D23FCB9AD");

        private readonly string rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        private readonly string pagesSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(NewsletterModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateSubscribers();
            CreateWidget();
            CreateWidgetOptions();
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the subscribers.
        /// </summary>
        private void CreateSubscribers()
        {
            Create
                .Table("Subscribers").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Email").AsString(MaxLength.Email).NotNullable();
        }

        /// <summary>
        /// Creates the widget.
        /// </summary>
        private void CreateWidget()
        {
            Insert
                .IntoTable("Contents").InSchema(rootSchemaName)
                .Row(new
                         {
                             Id = widgetId,
                             IsDeleted = false,
                             CreatedOn = DateTime.Now,
                             ModifiedOn = DateTime.Now,
                             CreatedByUser = "Admin",
                             ModifiedByUser = "Admin", 
                             Version = 1,
                             Name = "Newsletter Widget",
                             Status = 3,
                             PublishedOn = DateTime.Now,
                             PublishedByUser = "Admin"
                         });

            Insert
                .IntoTable("Widgets").InSchema(rootSchemaName)
                .Row(new
                {
                    Id = widgetId
                });

            Insert
                .IntoTable("ServerControlWidgets").InSchema(pagesSchemaName)
                .Row(new
                {
                    Id = widgetId,
                    Url = "~/Areas/bcms-newsletter/Views/Widgets/SubscribeToNewsletter.cshtml"
                });
        }

        /// <summary>
        /// Creates the widget options.
        /// </summary>
        private void CreateWidgetOptions()
        {
            Insert
                .IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(new
                {
                    Id = option1Id,
                    ContentId = widgetId,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    CreatedByUser = "Admin",
                    ModifiedByUser = "Admin",
                    Version = 1,
                    Key = "Email placeholder",
                    Type = 1,
                    DefaultValue = "email..."
                });

            Insert
                .IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(new
                {
                    Id = option2Id,
                    ContentId = widgetId,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    CreatedByUser = "Admin",
                    ModifiedByUser = "Admin",
                    Version = 1,
                    Key = "Label title",
                    Type = 1,
                    DefaultValue = "Subscribe to newsletter"
                });

            Insert
                .IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(new
                {
                    Id = option3Id,
                    ContentId = widgetId,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    CreatedByUser = "Admin",
                    ModifiedByUser = "Admin",
                    Version = 1,
                    Key = "Submit title",
                    Type = 1,
                    DefaultValue = "Submit"
                });
        }
    }
}