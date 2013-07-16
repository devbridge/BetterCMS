using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

using BetterCms.Core.Environment.Assemblies;
using BetterCms.Core.Modules;

using Common.Logging;

using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.Oracle;
using FluentMigrator.Runner.Processors.Postgres;
using FluentMigrator.Runner.Processors.SqlServer;

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
        private readonly IAssemblyLoader assemblyLoader;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly ICmsConfiguration configuration;

        private readonly IVersionChecker versionChecker;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMigrationRunner" /> class.
        /// </summary>
        /// <param name="assemblyLoader">The assembly loader.</param>
        /// <param name="configuration">The configuration.</param>
        public DefaultMigrationRunner(IAssemblyLoader assemblyLoader, ICmsConfiguration configuration, IVersionChecker versionChecker)
        {
            this.assemblyLoader = assemblyLoader;
            this.configuration = configuration;
            this.versionChecker = versionChecker;
        }

        /// <summary>
        /// Runs migrations from the specified assemblies.
        /// </summary>
        public void MigrateStructure(IList<ModuleDescriptor> moduleDescriptors)
        {
            IList<long> versions = new List<long>();
            IDictionary<ModuleDescriptor, IList<Type>> moduleWithMigrations = new Dictionary<ModuleDescriptor, IList<Type>>();

            foreach (var moduleDescriptor in moduleDescriptors)
            {
                var migrationTypes = assemblyLoader.GetLoadableTypes(moduleDescriptor.GetType().Assembly, typeof(Migration));
                if (migrationTypes != null)
                {
                    var types = migrationTypes as IList<Type> ?? migrationTypes.ToList();
                    moduleWithMigrations.Add(moduleDescriptor, types);

                    foreach (var migrationType in types)
                    {
                        var migrationAttributes = migrationType.GetCustomAttributes(typeof(MigrationAttribute), true);
                        if (migrationAttributes.Length > 0)
                        {
                            var attribute = migrationAttributes[0] as MigrationAttribute;
                            if (attribute != null)
                            {
                                versions.Add(attribute.Version);
                            }
                        }
                    }
                }
            }

            versions = versions.OrderBy(f => f).ToList();

            foreach (var version in versions)
            {
                foreach (var moduleWithMigration in moduleWithMigrations)
                {
                    if (!versionChecker.VersionExists(moduleWithMigration.Key.Name, version))
                    {
                        Migrate(moduleWithMigration.Key, moduleWithMigration.Value, version);
                        versionChecker.AddVersion(moduleWithMigration.Key.Name, version);
                    }
                }                    
            }
        }

        /// <summary>
        /// Runs database migrations of the specified module descriptor.
        /// </summary>
        /// <param name="moduleDescriptor">The module descriptor.</param>        
        /// <param name="up">if set to <c>true</c> migrates up; otherwise migrates down.</param>
        private void Migrate(ModuleDescriptor moduleDescriptor, IEnumerable<Type> migrationTypes = null, long? version = null)
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

            if (migrationTypes == null)
            {
                migrationTypes = assemblyLoader.GetLoadableTypes(assembly, typeof(Migration));
            }

            if (migrationTypes == null || !migrationTypes.Any())
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
            IDbConnection dbConnection = null;

            string connectionString;
            if (!string.IsNullOrEmpty(configuration.Database.ConnectionString))
            {
                connectionString = configuration.Database.ConnectionString;
            }
            else if (!string.IsNullOrEmpty(configuration.Database.ConnectionStringName))
            {
                connectionString = ConfigurationManager.ConnectionStrings[configuration.Database.ConnectionStringName].ConnectionString;
            }
            else
            {
                throw new ConfigurationErrorsException("Missing connection string.");
            }

            if (databaseType == DatabaseType.SqlAzure || databaseType == DatabaseType.SqlServer)
            {
                var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServer2008ProcessorFactory();
                processor = factory.Create(connectionString, announcer, options);
                dbConnection = ((SqlServerProcessor)processor).Connection;
            }
            else if (databaseType == DatabaseType.PostgreSQL)
            {
                var factory = new FluentMigrator.Runner.Processors.Postgres.PostgresProcessorFactory();
                processor = factory.Create(connectionString, announcer, options);
                dbConnection = ((PostgresProcessor)processor).Connection;
            }
            else if (databaseType == DatabaseType.Oracle)
            {
                var factory = new FluentMigrator.Runner.Processors.Oracle.OracleProcessorFactory();
                processor = factory.Create(connectionString, announcer, options);
                dbConnection = ((OracleProcessor)processor).Connection;
            }
            else
            {
                throw new NotSupportedException(string.Format("Database type {0} is not supported for data migrations.", databaseType));
            }
            
            var runner = new MigrationRunner(assembly, migrationContext, processor);

            if (version != null)
            {
                runner.MigrateUp(version.Value);
            }
            else
            {
                throw new NotSupportedException("Migrations without target version are not supported.");
            }

            // If connection is still opened, close it.
            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
            }
        }
    }
}
