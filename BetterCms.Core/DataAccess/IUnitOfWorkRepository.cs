using BetterCms.Core.DataAccess.DataContext;

namespace BetterCms.Core.DataAccess
{ 
    public interface IUnitOfWorkRepository
    {        
        void Use(IUnitOfWork unitOfWork);
    }
}
