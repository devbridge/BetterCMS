using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Newsletter.Models.Migrations
{
    [Migration(201304221200)]
    public class InitialSetup : DefaultMigration
    {
        public InitialSetup()
            : base(NewsletterModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateSubscribers();
        }

        public override void Down()
        {
            RemoveSubscribers();
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
    }
}