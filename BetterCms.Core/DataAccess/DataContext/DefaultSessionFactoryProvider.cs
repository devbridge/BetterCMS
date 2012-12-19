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

        public ISessionFactory SessionFactory
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
            var listener = new SaveOrUpdateEventListener(eventListenerHelper);

            fluentConfiguration = fluentConfiguration
                .Mappings(m => m.FluentMappings
                                   .Conventions.Add(ForeignKey.EndsWith("Id"))
                                   .Conventions.Add<EnumConvention>())
                .ExposeConfiguration(c => c.SetProperty("show_sql", "false"))
                .ExposeConfiguration(c => c.SetInterceptor(new StaleInterceptor()))
                .ExposeConfiguration(c => c.SetListener(ListenerType.Delete, new DeleteEventListener(eventListenerHelper)))
                .ExposeConfiguration(c => c.SetListener(ListenerType.SaveUpdate, listener))
                .ExposeConfiguration(c => c.SetListener(ListenerType.Save, listener))
                .ExposeConfiguration(c => c.SetListener(ListenerType.Update, listener));
            
            return fluentConfiguration
                        .BuildConfiguration()
                        .BuildSessionFactory();
        }
    }
}