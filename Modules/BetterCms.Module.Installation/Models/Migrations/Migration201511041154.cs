using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Installation.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201511041154)]
    public class Migration201511041154 : DefaultMigration
    {
        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201511041154"/> class.
        /// </summary>
        public Migration201511041154()
            : base(InstallationModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateDisqusWidgetOptions();
        }

        /// <summary>
        /// Creates the widget options.
        /// </summary>
        private void CreateDisqusWidgetOptions()
        {
            var options = new
            {
                DisqusCategoryId = new
                {
                    Id = "111CD45E-1CEB-4AD7-96F6-D00D68EA25CC",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = "2DFA000C-5FFE-45FF-98C9-320A942D86EF",
                    Key = "DisqusCategoryId",
                    Type = 1, // Text
                    DefaultValue = String.Empty,
                    IsDeletable = false
                },
                DisqusShortName = new
                {
                    Id = "4742CDBC-54AA-4C38-9738-A42BDE1FFE95",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = "2DFA000C-5FFE-45FF-98C9-320A942D86EF",
                    Key = "DisqusShortName",
                    Type = 1, // Text
                    DefaultValue = String.Empty,
                    IsDeletable = false
                }
            };

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.DisqusCategoryId);
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.DisqusShortName);
        }
    }
}