using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterCms.Module.Search.Content.Resources;

using FluentMigrator;

namespace BetterCms.Module.Search.Models.Migrations
{
    [Migration(201401301000)]
    public class Migration201401301000: DefaultMigration
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
        /// Initializes a new instance of the <see cref="Migration201401301000"/> class.
        /// </summary>
        public Migration201401301000()
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
                        TotalCountMessage =
                            new
                                {
                                    Id = "D7E1CA31-F522-415A-8854-6AF8FC3F1ED1",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    ContentId = "663A1D0C-FADA-4ACC-A34F-7437523AE65B",
                                    Key = SearchModuleConstants.WidgetOptionNames.TotalCountMessage,
                                    Type = 1, // Text
                                    DefaultValue = SearchGlobalization.SearchResults_TotalCount_Message,
                                    IsDeletable = false
                                }
                    };

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.TotalCountMessage);
        }
    }
}