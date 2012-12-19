namespace BetterCms.Core.DataAccess.DataContext
{
    public class DefaultUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ISessionFactoryProvider sessionFactoryProvider;

        public DefaultUnitOfWorkFactory(ISessionFactoryProvider sessionFactoryProvider)
        {
            this.sessionFactoryProvider = sessionFactoryProvider;
        }

        public IUnitOfWork New()
        {
            return new DefaultUnitOfWork(sessionFactoryProvider);
        }
    }
}
