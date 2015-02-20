using System.Web;

using BetterCms.Core.Web;

using BetterModules.Core.Web.Web;

namespace BetterCms.Module.Root.Services
{
    public class DefaultEntityTrackingCacheService : IEntityTrackingCacheService
    {
        private readonly IHttpContextAccessor contextAccessor;

        public DefaultEntityTrackingCacheService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public bool GetEntity(System.Type type, System.Guid id, out object entity)
        {
            entity = null;

            var context = GetCurrentContext();
            var key = CreateKey(type, id);
            if (context != null)
            {
                entity = context.Items[key];
                return entity != null;
            }

            return false;
        }

        public void AddEntity(System.Type type, System.Guid id, object entity)
        {
            if (entity == null)
            {
                return;
            }

            var context = GetCurrentContext();
            var key = CreateKey(type, id);

            if (context != null)
            {
                if (context.Items.Contains(key))
                {
                    context.Items.Remove(key);
                }

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