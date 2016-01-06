using System;

using BetterCms.Module.Root.Models.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201502101138)]
    public class Migration201502101138 : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201502101138"/> class.
        /// </summary>
        public Migration201502101138() : base(PagesModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            if (Schema.Schema(rootModuleSchemaName).Table("CategorizableItems").Exists())
            {
                Insert.IntoTable("CategorizableItems").InSchema(rootModuleSchemaName).Row(new
                {

                    Id = "DC861498-FCD1-4F19-9C75-AE71916EF7BF",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    Name = PageProperties.CategorizableItemKeyForPages
                });
            }

            MigrateCategoryData();
        }

        private void MigrateCategoryData()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201502101138.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }
    }
}