using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Search.Models.Migrations
{
    [Migration(201401080830)]
    public class Migration201401080830: DefaultMigration
    {
        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// The better CMS user name
        /// </summary>
        private const string BetterCmsUserName = "Better CMS";

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201401080830"/> class.
        /// </summary>
        public Migration201401080830()
            : base(SearchModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Migrates up current module.
        /// </summary>
        public override void Up()
        {
            var dateNow = DateTime.Now;

            var options = new
                    {
                        ResultsCount =
                            new
                                {
                                    Id = "6E0949B4-3339-4984-8103-D98F3FBC031D",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    ContentId = "663A1D0C-FADA-4ACC-A34F-7437523AE65B",
                                    Key = SearchModuleConstants.WidgetOptionNames.ResultsCount,
                                    Type = 2, // Integer
                                    DefaultValue = SearchModuleConstants.DefaultSearchResultsCount,
                                    IsDeletable = false
                                },
                        ShowTotalResults =
                            new
                                {
                                    Id = "33384E55-1B47-4040-A104-165F2979A7A0",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    ContentId = "663A1D0C-FADA-4ACC-A34F-7437523AE65B",
                                    Key = SearchModuleConstants.WidgetOptionNames.ShowTotalResults,
                                    Type = 5, // Boolean
                                    DefaultValue = "true",
                                    IsDeletable = false
                                }
                    };

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.ResultsCount)
                .Row(options.ShowTotalResults);
        }
    }
}