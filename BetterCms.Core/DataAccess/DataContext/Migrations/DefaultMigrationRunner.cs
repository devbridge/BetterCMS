using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using BetterCms.Core.Environment.Assemblies;
using BetterCms.Core.Modules;
using System;

using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using Common.Logging;

using Microsoft.CSharp;

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
        /// Runs migrations from the specified assemblies.
        /// </summary>
        public void Migrate(IList<ModuleDescriptor> moduleDescriptors, bool up)
        {
            List<Tuple<long, ModuleDescriptor>> versions = new List<Tuple<long, ModuleDescriptor>>();

            foreach (var moduleDescriptor in moduleDescriptors)
            {
                var migrationTypes = assemblyLoader.GetLoadableTypes(moduleDescriptor.GetType().Assembly, typeof(Migration));
                foreach (var migrationType in migrationTypes)
                {                  
                  var migrationAttributes = migrationType.GetCustomAttributes(typeof(MigrationAttribute), true);
                    if (migrationAttributes.Length > 0)
                    {
                        var attribute = migrationAttributes[0] as MigrationAttribute;
                        if (attribute != null)
                        {
                            versions.Add(new Tuple<long, ModuleDescriptor>(attribute.Version, moduleDescriptor));
                        }
                    }
                }        
            }

            if (up)
            {
                versions = versions.OrderBy(f => f.Item1).ThenByDescending(f => f.Item2.Order).ToList();
            }
            else
            {
                versions = versions.OrderByDescending(f => f.).ThenBy(f => f.Value.Order).ToList();
            }

            foreach (var version in versions)
            {
                Migrate(version.Value, up, version.Key);
            }
        }

        /// <summary>
        /// Runs database migrations of the specified module descriptor.
        /// </summary>
        /// <param name="moduleDescriptor">The module descriptor.</param>        
        /// <param name="up">if set to <c>true</c> migrates up; otherwise migrates down.</param>
        public void Migrate(ModuleDescriptor moduleDescriptor, bool up = true, long? version = null)
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

            var migrationTypes = assemblyLoader.GetLoadableTypes(assembly, typeof(Migration)).ToList();

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
            runner.ValidateVersionOrder();

            if (up)
            {
                if (version != null)
                {
                    runner.MigrateUp(version.Value);
                }
                else
                {
                    runner.MigrateUp();
                }
            }
            else
            {               
                runner.MigrateDown(version ?? 0);                
            }
        }


    }
}
