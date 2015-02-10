using Devbridge.Platform.Core.DataAccess.DataContext;

namespace Devbridge.Platform.Core.DataAccess
{ 
    public interface IUnitOfWorkRepository
    {        
        void Use(IUnitOfWork unitOfWork);
    }
}
