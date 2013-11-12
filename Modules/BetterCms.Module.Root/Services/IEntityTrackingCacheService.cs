using System;

namespace BetterCms.Module.Root.Services
{
    public interface IEntityTrackingCacheService
    {
        object GetEntity(Type type, Guid id);
        
        void AddEntity(Type type, Guid id, object entity);
    }
}
