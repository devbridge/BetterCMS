using System;

namespace BetterCms.Module.Root.Services
{
    public interface IEntityTrackingCacheService
    {
        bool GetEntity(Type type, Guid id, out object entity);
        
        void AddEntity(Type type, Guid id, object entity);
    }
}
