using System;

using BetterCms.Module.Search.Content.Resources;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Search.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201401030900)]
    public class InitialSetup : DefaultMigration
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
        /// The better CMS user name
        /// </summary>
        private const string BetterCmsUserName = "Better CMS";

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(SearchModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
            pagesSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Migrates up current module.
        /// </summary>
        public override void Up()
        {
            // Creates widget for displaying search form
            CreateSearchInputWidget();

            // Creates widget for displaying search results
            CreateSearchResultsWidget();

            // Creates search category and assigns widgets to that category
            CreateSearchCategory();
        }

        private void CreateSearchInputWidget()
        {
            var dateNow = DateTime.Now;

            var widget = new
                    {
                        ForRootSchemaContentTable = new
                                {
                                    Id = "D31DB767-B352-4E5B-A0DA-6696A53B87F6",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    Name = "Search input form",
                                    Status = 3,
                                    PublishedOn = dateNow,
                                    PublishedByUser = BetterCmsUserName
                                },
                        ForRootScemaWidgetsTable = new
                                {
                                    Id = "D31DB767-B352-4E5B-A0DA-6696A53B87F6"
                                },
                        ForPagesSchemaServerControlWidgetsTable = new
                                {
                                    Id = "D31DB767-B352-4E5B-A0DA-6696A53B87F6", 
                                    Url = "~/Areas/bcms-search/Views/Search/SearchInputWidget.cshtml"
                                }
                    };

            var options = new
                    {
                        FormMethod = new
                                {
                                    Id = "55AC343E-E17B-4E45-8292-0592CDBB1046",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    ContentId = widget.ForRootSchemaContentTable.Id,
                                    Key = SearchModuleConstants.WidgetOptionNames.FormMethod,
                                    Type = 1, // Text
                                    DefaultValue = SearchModuleConstants.WidgetOptionDefaultValues.FormMethod,
                                    IsDeletable = false
                                },
                        InputPlaceholder = new
                                {
                                    Id = "4C9D7949-5986-488C-85F7-CBA92F2C983B",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    ContentId = widget.ForRootSchemaContentTable.Id,
                                    Key = SearchModuleConstants.WidgetOptionNames.InputPlaceholder,
                                    Type = 1, // Text
                                    DefaultValue = SearchGlobalization.SearchForm_InputPlaceholder_Title,
                                    IsDeletable = false
                                },
                        LabelTitle = new
                                {
                                    Id = "4B4C18E6-F946-407A-81A4-FF5BD58B42E5",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    ContentId = widget.ForRootSchemaContentTable.Id,
                                    Key = SearchModuleConstants.WidgetOptionNames.LabelTitle,
                                    Type = 1, // Text
                                    IsDeletable = false
                                },
                        QueryParameterName = new
                                {
                                    Id = "2AA3515A-5059-41A9-9A82-1CCE2D110D8D",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    ContentId = widget.ForRootSchemaContentTable.Id,
                                    Key = SearchModuleConstants.WidgetOptionNames.QueryParameterName,
                                    Type = 1, // Text
                                    DefaultValue = SearchModuleConstants.WidgetOptionDefaultValues.QueryParameterName,
                                    IsDeletable = false
                                },
                        SearchResultsUrl = new
                                {
                                    Id = "E171410E-587E-43FE-9A96-60B148D0B9F9",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    ContentId = widget.ForRootSchemaContentTable.Id,
                                    Key = SearchModuleConstants.WidgetOptionNames.SearchResultsUrl,
                                    Type = 1, // Text
                                    IsDeletable = false
                                },
                        SubmitTitle = new
                                {
                                    Id = "36B53399-EDAE-4D85-9089-A96FE37D3FD9",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    ContentId = widget.ForRootSchemaContentTable.Id,
                                    Key = SearchModuleConstants.WidgetOptionNames.SubmitTitle,
                                    Type = 1, // Text
                                    DefaultValue = SearchGlobalization.SearchForm_SubmitButton_Title,
                                    IsDeletable = false
                                }
                    };

            // Register server control widget.
            Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(widget.ForRootSchemaContentTable);
            Insert.IntoTable("Widgets").InSchema(rootSchemaName).Row(widget.ForRootScemaWidgetsTable);
            Insert.IntoTable("ServerControlWidgets").InSchema(pagesSchemaName).Row(widget.ForPagesSchemaServerControlWidgetsTable);

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.FormMethod)
                .Row(options.InputPlaceholder)
                .Row(options.LabelTitle)
                .Row(options.QueryParameterName)
                .Row(options.SearchResultsUrl)
                .Row(options.SubmitTitle);
        }

        private void CreateSearchResultsWidget()
        {
            var dateNow = DateTime.Now;

            var widget = new
                    {
                        ForRootSchemaContentTable = new
                                {
                                    Id = "663A1D0C-FADA-4ACC-A34F-7437523AE65B",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    Name = "Search results",
                                    Status = 3,
                                    PublishedOn = dateNow,
                                    PublishedByUser = BetterCmsUserName
                                },
                        ForRootScemaWidgetsTable = new
                                {
                                    Id = "663A1D0C-FADA-4ACC-A34F-7437523AE65B"
                                },
                        ForPagesSchemaServerControlWidgetsTable = new
                                {
                                    Id = "663A1D0C-FADA-4ACC-A34F-7437523AE65B",
                                    Url = "~/Areas/bcms-search/Views/Search/SearchResultsWidgetInvoker.cshtml"
                                }
                    };


            var options = new
                    {
                        QueryParameterName = new
                                {
                                    Id = "843EC044-78C5-4A31-94CA-8284BD1E81AB",
                                    Version = 1,
                                    IsDeleted = false,
                                    CreatedOn = dateNow,
                                    CreatedByUser = BetterCmsUserName,
                                    ModifiedOn = dateNow,
                                    ModifiedByUser = BetterCmsUserName,
                                    ContentId = widget.ForRootSchemaContentTable.Id,
                                    Key = SearchModuleConstants.WidgetOptionNames.QueryParameterName,
                                    Type = 1, // Text
                                    DefaultValue = SearchModuleConstants.WidgetOptionDefaultValues.QueryParameterName,
                                    IsDeletable = false
                                }
                    };

            // Register server control widget.
            Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(widget.ForRootSchemaContentTable);
            Insert.IntoTable("Widgets").InSchema(rootSchemaName).Row(widget.ForRootScemaWidgetsTable);
            Insert.IntoTable("ServerControlWidgets").InSchema(pagesSchemaName).Row(widget.ForPagesSchemaServerControlWidgetsTable);

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.QueryParameterName);
        }

        private void CreateSearchCategory()
        {
            IfSqlServer().Execute.EmbeddedScript("InitialSetup.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }
    }
}