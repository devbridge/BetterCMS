using System;
using BetterCms.Core.DataAccess.DataContext.Conventions;
using BetterCms.Core.DataAccess.DataContext.EventListeners;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Services;

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
            MsSqlConfiguration sqlConfiguration = null;

            if (!string.IsNullOrEmpty(configuration.Database.ConnectionString))
            {
                sqlConfiguration = MsSqlConfiguration.MsSql2008.ConnectionString(configuration.Database.ConnectionString); 
            }
            else if (!string.IsNullOrEmpty(configuration.Database.ConnectionStringName))
            {                
                sqlConfiguration = MsSqlConfiguration.MsSql2008.ConnectionString(f => f.FromConnectionStringWithKey(configuration.Database.ConnectionStringName));
            }

            if (sqlConfiguration != null)
            {
                if (!string.IsNullOrEmpty(configuration.Database.SchemaName))
                {
                    sqlConfiguration.DefaultSchema(configuration.Database.SchemaName);
                }

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
    }
}