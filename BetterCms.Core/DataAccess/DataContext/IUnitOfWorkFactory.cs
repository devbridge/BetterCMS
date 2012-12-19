namespace BetterCms.Core.DataAccess.DataContext
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork New();
    }
}
