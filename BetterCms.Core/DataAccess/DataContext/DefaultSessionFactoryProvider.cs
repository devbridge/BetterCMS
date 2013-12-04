using System;
using System.Data.SqlClient;

using BetterCms.Configuration;
using BetterCms.Core.DataAccess.DataContext.Conventions;
using BetterCms.Core.DataAccess.DataContext.EventListeners;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Services;

using Common.Logging;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using BetterCms.Core.DataAccess.DataContext.Interceptors;

using NHibernate.Event;

namespace BetterCms.Core.DataAccess.DataContext
{
    public class DefaultSessionFactoryProvider : ISessionFactoryProvider
    {
        private static readonly object lockObject = new object();
        private readonly ICmsConfiguration configuration;
        private readonly IMappingResolver mappingResolver;
        private volatile ISessionFactory sessionFactory;
        private readonly ISecurityService securityService;

        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        public DefaultSessionFactoryProvider(IMappingResolver mappingResolver, ICmsConfiguration configuration, ISecurityService securityService)
        {
            this.mappingResolver = mappingResolver;
            this.configuration = configuration;
            this.securityService = securityService;            
        }

        /// <summary>
        /// Opens the session.
        /// </summary>
        /// <param name="trackEntitiesConcurrency">if set to <c>true</c> tracks entities concurrency.</param>
        /// <returns>An opened session object.</returns>        
        public ISession OpenSession(bool trackEntitiesConcurrency = true)
        {
            if (trackEntitiesConcurrency)
            {
                return SessionFactory.OpenSession(new StaleInterceptor());
            }

            return SessionFactory.OpenSession();
        }

        protected virtual ISessionFactory SessionFactory
        {
            get
            {
                try
                {
                    if (sessionFactory == null)
                    {
                        lock (lockObject)
                        {
                            if (sessionFactory == null)
                            {
                                sessionFactory = CreateSessionFactory();
                            }
                        }
                    }

                    return sessionFactory;
                }
                catch (Exception ex)
                {
                    throw new DataTierException("Failed to initialize NHibernate session factory.", ex);
                }
            }
        }

        private ISessionFactory CreateSessionFactory()
        {   
            FluentConfiguration fluentConfiguration = Fluently.Configure();

            IPersistenceConfigurer sqlConfiguration = CreateSqlConfiguration();
            if (sqlConfiguration != null)
            {
                fluentConfiguration = fluentConfiguration.Database(sqlConfiguration);
            }

            mappingResolver.AddAvailableMappings(fluentConfiguration);

            var eventListenerHelper = new EventListenerHelper(securityService);
            var saveOrUpdateEventListener = new SaveOrUpdateEventListener(eventListenerHelper);
            var deleteEventListener = new DeleteEventListener(eventListenerHelper);

            fluentConfiguration = fluentConfiguration
                .Mappings(m => m.FluentMappings
                                   .Conventions.Add(ForeignKey.EndsWith("Id"))
                                   .Conventions.Add<EnumConvention>())
                .ExposeConfiguration(c => c.SetProperty("show_sql", "false"))              
                .ExposeConfiguration(c => c.SetListener(ListenerType.Delete, deleteEventListener))
                .ExposeConfiguration(c => c.SetListener(ListenerType.SaveUpdate, saveOrUpdateEventListener))
                .ExposeConfiguration(c => c.SetListener(ListenerType.Save, saveOrUpdateEventListener))
                .ExposeConfiguration(c => c.SetListener(ListenerType.Update, saveOrUpdateEventListener));
            
            return fluentConfiguration
                        .BuildConfiguration()
                        .BuildSessionFactory();
        }

        /// <summary>
        /// Creates the SQL configuration.
        /// </summary>
        /// <returns>Created SQL configuration</returns>
        private IPersistenceConfigurer CreateSqlConfiguration()
        {
            IPersistenceConfigurer sqlConfiguration;

            switch (configuration.Database.DatabaseType)
            {
                case DatabaseType.MsSql2008:
                    sqlConfiguration = CreateSqlConfiguration(MsSqlConfiguration.MsSql2008);
                    break;
                case DatabaseType.MsSql2005:
                    sqlConfiguration = CreateSqlConfiguration(MsSqlConfiguration.MsSql2005);
                    break;
                case DatabaseType.MsSql2000:
                    sqlConfiguration = CreateSqlConfiguration(MsSqlConfiguration.MsSql2000);
                    break;
                case DatabaseType.Oracle10:
                    sqlConfiguration = CreateSqlConfiguration(OracleDataClientConfiguration.Oracle10);
                    break;
                case DatabaseType.Oracle9:
                    sqlConfiguration = CreateSqlConfiguration(OracleDataClientConfiguration.Oracle9);
                    break;
                case DatabaseType.PostgreSQL81:
                    sqlConfiguration = CreateSqlConfiguration(PostgreSQLConfiguration.PostgreSQL82);
                    break;
                case DatabaseType.PostgreSQL82:
                    sqlConfiguration = CreateSqlConfiguration(PostgreSQLConfiguration.PostgreSQL81);
                    break;
                case DatabaseType.PostgreSQLStandard:
                    sqlConfiguration = CreateSqlConfiguration(PostgreSQLConfiguration.Standard);
                    break;
                default:
                    throw new NotImplementedException(string.Format("Unknown DatabaseType: {0}", configuration.Database.DatabaseType));
            }

            return sqlConfiguration;
        }

        /// <summary>
        /// Creates the SQL configuration.
        /// </summary>
        /// <typeparam name="TThisConfiguration">The type of the configuration.</typeparam>
        /// <typeparam name="TConnectionString">The type of connection string.</typeparam>
        /// <param name="provider">The SQL configuration provider.</param>
        /// <returns>Created SQL configuration</returns>
        private IPersistenceConfigurer CreateSqlConfiguration<TThisConfiguration, TConnectionString>(PersistenceConfiguration<TThisConfiguration, TConnectionString> provider)
            where TThisConfiguration : PersistenceConfiguration<TThisConfiguration, TConnectionString>
            where TConnectionString : ConnectionStringBuilder, new()
        {
            PersistenceConfiguration<TThisConfiguration, TConnectionString> sqlConfiguration;

            if (!string.IsNullOrEmpty(configuration.Database.ConnectionString))
            {
                sqlConfiguration = provider.ConnectionString(configuration.Database.ConnectionString);
            }
            else if (!string.IsNullOrEmpty(configuration.Database.ConnectionStringName))
            {
                sqlConfiguration = provider.ConnectionString(f => f.FromConnectionStringWithKey(configuration.Database.ConnectionStringName));
            }
            else
            {
                sqlConfiguration = null;
            }

            if (sqlConfiguration != null)
            {
                if (!string.IsNullOrEmpty(configuration.Database.SchemaName))
                {
                    sqlConfiguration.DefaultSchema(configuration.Database.SchemaName);
                }

                if (!string.IsNullOrEmpty(configuration.Database.ConnectionProvider))
                {
                    sqlConfiguration.Provider(configuration.Database.ConnectionProvider);
                }
            }

            return sqlConfiguration;
        }

        ~DefaultSessionFactoryProvider()
        {
            try
            {
                if (sessionFactory != null)
                {
                    if (!sessionFactory.IsClosed)
                    {
                        sessionFactory.Close();
                    }
                    sessionFactory.Dispose();
                }
            }
            catch (Exception exc)
            {
                Logger.Error("Unhandled exception occurred when disposing Session Factory Provider.", exc);
            }
            finally
            {
                SqlConnection.ClearAllPools();
            }
        }
    }
}