using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Newsletter.Models.Migrations
{
    [Migration(201304221200)]
    public class InitialSetup : DefaultMigration
    {
        private static readonly Guid widgetId = new Guid("C7F20821-E9FF-43AE-B8A0-63858447A72B");
        private static readonly Guid option1Id = new Guid("84E5C6DB-2801-4294-87AA-8ACA295EB792");
        private static readonly Guid option2Id = new Guid("44C9DAC8-1924-4F9C-9829-68E3A4B0D384");
        private static readonly Guid option3Id = new Guid("CD2DDAE1-F128-4AF2-B141-954D23FCB9AD");
        private static readonly Guid option4Id = new Guid("06B8ED9F-5F40-4FE6-ACA6-BAD008F02AF9");

        private readonly string rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        private readonly string pagesSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;

        public InitialSetup()
            : base(NewsletterModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateSubscribers();
            CreateWidget();
            CreateWidgetOptions();
        }

        public override void Down()
        {
            RemoveSubscribers();
            DeleteWidget();
            DeleteWidgetOptions();
        }

        private void CreateSubscribers()
        {
            Create
                .Table("Subscribers").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Email").AsString(MaxLength.Email).NotNullable();
        }

        private void RemoveSubscribers()
        {
            Delete.Table("Subscribers").InSchema(SchemaName);
        }

        private void CreateWidget()
        {
            Insert
                .IntoTable("Contents").InSchema(rootSchemaName)
                .Row(new
                         {
                             Id = widgetId, 
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

        private void DeleteWidget()
        {
            Update
                .Table("Contents").InSchema(rootSchemaName)
                .Set(new {IsDeleted = 1, DeletedOn = DateTime.Now, DeletedByUser = "Admin"})
                .Where(new {Id = widgetId, IsDeleted = 0});
        }

        private void CreateWidgetOptions()
        {
            Insert
                .IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(new
                {
                    Id = option1Id,
                    ContentId = widgetId,
                    CreatedByUser = "Admin",
                    ModifiedByUser = "Admin",
                    Version = 1,
                    Key = "BcmsNewsletterSubscribeEmailFieldName",
                    Type = 1,
                    DefaultValue = "SubscriberEmail"
                });
            
            Insert
                .IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(new
                {
                    Id = option2Id,
                    ContentId = widgetId,
                    CreatedByUser = "Admin",
                    ModifiedByUser = "Admin",
                    Version = 1,
                    Key = "BcmsNewsletterSubscribeEmailFieldPlaceholder",
                    Type = 1,
                    DefaultValue = "email..."
                });

            Insert
                .IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(new
                {
                    Id = option3Id,
                    ContentId = widgetId,
                    CreatedByUser = "Admin",
                    ModifiedByUser = "Admin",
                    Version = 1,
                    Key = "BcmsNewsletterSubscribeLabelTitle",
                    Type = 1,
                    DefaultValue = "Subscribe to newsletter"
                });

            Insert
                .IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(new
                {
                    Id = option4Id,
                    ContentId = widgetId,
                    CreatedByUser = "Admin",
                    ModifiedByUser = "Admin",
                    Version = 1,
                    Key = "BcmsNewsletterSubscribeFormMethod",
                    Type = 1,
                    DefaultValue = "POST"
                });
        }

        private void DeleteWidgetOptions()
        {
            Update
                .Table("ContentOptions").InSchema(rootSchemaName)
                .Set(new {IsDeleted = 1, DeletedOn = DateTime.Now, DeletedByUser = "Admin"})
                .Where(new {Id = option1Id, IsDeleted = 0});

            Update
                .Table("ContentOptions").InSchema(rootSchemaName)
                .Set(new { IsDeleted = 1, DeletedOn = DateTime.Now, DeletedByUser = "Admin" })
                .Where(new { Id = option2Id, IsDeleted = 0 });

            Update
                .Table("ContentOptions").InSchema(rootSchemaName)
                .Set(new { IsDeleted = 1, DeletedOn = DateTime.Now, DeletedByUser = "Admin" })
                .Where(new { Id = option3Id, IsDeleted = 0 });
            
            Update
                .Table("ContentOptions").InSchema(rootSchemaName)
                .Set(new { IsDeleted = 1, DeletedOn = DateTime.Now, DeletedByUser = "Admin" })
                .Where(new { Id = option4Id, IsDeleted = 0 });
        }
    }
}