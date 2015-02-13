using System;

using Devbridge.Platform.Core.Exceptions;
using Devbridge.Platform.Core.Models;

using FluentMigrator;
using FluentMigrator.Builders.IfDatabase;

namespace Devbridge.Platform.Core.DataAccess.DataContext.Migrations
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
                return SchemaNameProvider.GetSchemaName(moduleName);
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
            throw new PlatformException("Down migration not possible.", new NotSupportedException("Application doesn't support DOWN migrations."));
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
