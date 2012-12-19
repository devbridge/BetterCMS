using System.Configuration;
using System.Linq;

using BetterCms.Core.Environment.Assemblies;
using BetterCms.Core.Modules;
using System;

using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using Common.Logging;

namespace BetterCms.Core.DataAccess.DataContext.Migrations
{    
    /// <summary>
    /// Default database migrations runner.
    /// </summary>
    public class DefaultMigrationRunner : IMigrationRunner
    {
        /// <summary>
        /// Database type to run migrations.
        /// TODO: move it to cms.config
        /// </summary>
        private const DatabaseType databaseType = DatabaseType.SqlAzure;

        /// <summary>
        /// Timeout for one migration execution.
        /// </summary>
        private static readonly TimeSpan migrationTimeout = new TimeSpan(0, 1, 0);

        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Provides assembly and types loading methods.
        /// </summary>
        private IAssemblyLoader assemblyLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMigrationRunner" /> class.
        /// </summary>
        /// <param name="assemblyLoader">The assembly loader.</param>
        public DefaultMigrationRunner(IAssemblyLoader assemblyLoader)
        {
            this.assemblyLoader = assemblyLoader;
        }

        /// <summary>
        /// Runs database migrations of the specified module descriptor.
        /// </summary>
        /// <param name="moduleDescriptor">The module descriptor.</param>        
        /// <param name="up">if set to <c>true</c> migrates up; otherwise migrates down.</param>
        public void Migrate(ModuleDescriptor moduleDescriptor, bool up = true)
        {
            var announcer = new TextWriterAnnouncer(
                s =>
                    {
                        if (!string.IsNullOrWhiteSpace(s))
                        {
                            Log.Info(string.Concat("Migration on ", moduleDescriptor.Name, ". ", s));
                        }
                    });

            var assembly = moduleDescriptor.GetType().Assembly;

            var migrationTypes = assemblyLoader.GetLoadableTypes(assembly, typeof(Migration));

            if (!migrationTypes.Any())
            {
                Log.Info(string.Concat("Migration on ", moduleDescriptor.Name, ". No migrations found."));
                return;
            }

            var migrationContext = new RunnerContext(announcer)
            {
                Namespace = migrationTypes.First().Namespace
            };            

            IMigrationProcessorOptions options = new ProcessorOptions
                {
                    PreviewOnly = false,
                    Timeout = (int)migrationTimeout.TotalSeconds
                };

            IMigrationProcessor processor;

            var connectionString = ConfigurationManager.ConnectionStrings["BetterCms"].ConnectionString;

            if (databaseType == DatabaseType.SqlAzure || databaseType == DatabaseType.SqlServer)
            {
                var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServer2008ProcessorFactory();
                processor = factory.Create(connectionString, announcer, options);                
            }
            else if (databaseType == DatabaseType.PostgreSQL)
            {
                var factory = new FluentMigrator.Runner.Processors.Postgres.PostgresProcessorFactory();
                processor = factory.Create(connectionString, announcer, options);
            }
            else if (databaseType == DatabaseType.Oracle)
            {
                var factory = new FluentMigrator.Runner.Processors.Oracle.OracleProcessorFactory();
                processor = factory.Create(connectionString, announcer, options);
            }
            else
            {
                throw new NotSupportedException(string.Format("Database type {0} is not supported for data migrations.", databaseType));
            }

            var runner = new MigrationRunner(assembly, migrationContext, processor);
            if (up)
            {
                runner.MigrateUp();
            }
            else
            {
                runner.MigrateDown(0);                
            }
        }
    }
}
