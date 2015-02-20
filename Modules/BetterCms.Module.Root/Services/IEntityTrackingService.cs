using BetterModules.Core.DataContracts;

namespace BetterCms.Module.Root.Services
{
    public interface IEntityTrackingService
    {
        void OnEntityUpdate(IEntity entity);

        void OnEntityDelete(IEntity entity);
    }
}
