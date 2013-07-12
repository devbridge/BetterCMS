using System;

using BetterCms.Core.Exceptions;

using FluentMigrator;
using FluentMigrator.Builders.IfDatabase;

namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    public abstract class DefaultMigration : Migration
    {
        protected const string PostgresThrowNotSupportedErrorSql = "RAISE EXCEPTION 'NOT SUPPORTED IN CURRENT VERSION!';";

        protected const string OracleThrowNotSupportedErrorSql = "raise_application_error(-1, 'NOT SUPPORTED IN CURRENT VERSION!');";

        private readonly string moduleName;

        public string SchemaName
        {
            get
            {
                return "bcms_" + moduleName;
            }
        }

        public DefaultMigration(string moduleName)
        {
            this.moduleName = moduleName;
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new CmsException("Down migration not possible.", new NotSupportedException("Better CMS don't supports DOWN migrations."));
        }

        protected IIfDatabaseExpressionRoot IfSqlServer()
        {
            return IfDatabase("SqlServer");
        }

        protected IIfDatabaseExpressionRoot IfPostgres()
        {
            return IfDatabase("Postgres");
        }

        protected IIfDatabaseExpressionRoot IfOracle()
        {
            return IfDatabase("Oracle");
        }
    }
}
