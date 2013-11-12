using System.Web;

using BetterCms.Core.Web;

namespace BetterCms.Module.Root.Services
{
    public class DefaultEntityTrackingCacheService : IEntityTrackingCacheService
    {
        private readonly IHttpContextAccessor contextAccessor;

        public DefaultEntityTrackingCacheService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public object GetEntity(System.Type type, System.Guid id)
        {
            var context = GetCurrentContext();
            var key = CreateKey(type, id);
            if (context != null)
            {
                return context.Items[key];
            }
            
            return null;
        }

        public void AddEntity(System.Type type, System.Guid id, object entity)
        {
            var context = GetCurrentContext();
            var key = CreateKey(type, id);

            if (context != null)
            {
                context.Items.Add(key, entity);
            }
        }

        private string CreateKey(System.Type type, System.Guid id)
        {
            return string.Format("{0}_{1}", type, id);
        }

        private HttpContextBase GetCurrentContext()
        {
            if (contextAccessor != null)
            {
                return contextAccessor.GetCurrent();
            }

            return null;
        }
    }
}