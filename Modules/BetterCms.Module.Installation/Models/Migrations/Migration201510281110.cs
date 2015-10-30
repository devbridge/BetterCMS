using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Installation.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201510281110)]
    public class Migration201510281110 : DefaultMigration
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
        /// Initializes a new instance of the <see cref="Migration201510281110"/> class.
        /// </summary>
        public Migration201510281110()
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
            CreateBlogCategoriesWidget();
        }

        /// <summary>
        /// Creates the widget.
        /// </summary>
        private void CreateBlogCategoriesWidget()
        {
            var widget = new
            {
                ForRootSchemaContentTable = new
                {
                    Id = "4110B9AA-8598-4A97-8EB6-A5830F6A43D1",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    Name = "Blog Posts Category List Widget",
                    Status = 3,
                    PublishedOn = DateTime.Now,
                    PublishedByUser = "Better CMS"
                },

                ForRootScemaWidgetsTable = new
                {
                    Id = "4110B9AA-8598-4A97-8EB6-A5830F6A43D1",
                    CategoryId = "973B0FA7-4633-4154-BA5D-49BCE5591CC0"
                },

                ForPagesSchemaServerControlWidgetsTable = new
                {
                    Id = "4110B9AA-8598-4A97-8EB6-A5830F6A43D1",
                    Url = "~/Areas/bcms-installation/Views/Widgets/BlogPostCategoriesWidgetInvoker.cshtml"
                },

                ForRootSchemaWidgetCategoriesTable = new
                {
                Id = "E22A6FFD-95A7-42A6-BEB5-5B85B03AB446",
                Version = 0,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                CreatedByUser = "Admin",
                ModifiedOn = DateTime.Now,
                ModifiedByUser = "Admin",
                WidgetId = "4110B9AA-8598-4A97-8EB6-A5830F6A43D1",
                CategoryId = "973B0FA7-4633-4154-BA5D-49BCE5591CC0"
                }
            };

            // Register server control widget.
            Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(widget.ForRootSchemaContentTable);
            Insert.IntoTable("Widgets").InSchema(rootSchemaName).Row(widget.ForRootScemaWidgetsTable);
            Insert.IntoTable("WidgetCategories").InSchema(rootSchemaName).Row(widget.ForRootSchemaWidgetCategoriesTable);
            Insert.IntoTable("ServerControlWidgets").InSchema(pagesSchemaName).Row(widget.ForPagesSchemaServerControlWidgetsTable);
        }
    }
}