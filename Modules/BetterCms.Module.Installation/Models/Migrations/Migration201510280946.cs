using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Installation.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201510280946)]
    public class Migration201510280946 : DefaultMigration
    {
        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// The pages schema name
        /// </summary>
        private readonly string pagesSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201510280946"/> class.
        /// </summary>
        public Migration201510280946()
            : base(InstallationModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
            pagesSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateBlogWidgetCategory();
        }

        /// <summary>
        /// Creates a Widget Category for Blog Widgets
        /// </summary>
        private void CreateBlogWidgetCategory()
        {
            var blogWidgetCategory = new
            {
                Id = "650EC2E9-052E-4A79-85B9-852CB7C14115",
                Version = 0,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                CreatedByUser = "Admin",
                ModifiedOn = DateTime.Now,
                ModifiedByUser = "Admin",
                WidgetId = "7218F62A-6C38-49EA-AF65-D610E4B3AE0A",
                CategoryId = "973B0FA7-4633-4154-BA5D-49BCE5591CC0"
            };

            Insert.IntoTable("WidgetCategories").InSchema(rootSchemaName).Row(blogWidgetCategory);
        }
    }
}