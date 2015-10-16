using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201502101136)]
    public class Migration201502101136 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201502101136"/> class.
        /// </summary>
        public Migration201502101136() : base(RootModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            if (Schema.Schema(SchemaName).Table("CategorizableItems").Exists())
            {
                Insert.IntoTable("CategorizableItems").InSchema(SchemaName).Row(new
                {

                    Id = "B2F05159-74AF-4B67-AEB9-36B9CC9EED57",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    Name = Widget.CategorizableItemKeyForWidgets
                });
            }

            MigrateCategoryData();
        }

        private void MigrateCategoryData()
        {
            IfSqlServer().Execute.EmbeddedScript("Migration201502101136.sqlserver.sql");

            // TODO: add Postgres support.
            IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

            // TODO: add Oracle support.
            IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
        }
    }
}