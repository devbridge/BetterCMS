using BetterCms.Api.Services;
using BetterCms.Core.Dependencies;

namespace BetterCms.Api
{
    public class CmsContext
    {
        private static CmsContext instance;

        private readonly ITagApiService tagService;

        public CmsContext(ITagApiService tagService)
        {
            this.tagService = tagService;
        }

        public ITagApiService TagService
        {
            get
            {
                return tagService;
            }
        }

        public static CmsContext Api
        {
            get
            {
                if (instance == null)
                {
                    instance = ContextScopeProvider.Resolve<CmsContext>();
                }
                return instance;
            }
        }
    }
}