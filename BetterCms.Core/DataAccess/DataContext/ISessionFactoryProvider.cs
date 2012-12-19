using NHibernate;

namespace BetterCms.Core.DataAccess.DataContext
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory SessionFactory { get; }
    }
}
