namespace Devbridge.Platform.Core.DataAccess.DataContext
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork New();
    }
}
