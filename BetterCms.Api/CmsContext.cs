using BetterCms.Api.Services;
using BetterCms.Core.Dependencies;

namespace BetterCms.Api
{
    public class CmsContext
    {
        private static CmsContext instance;

        private readonly ITagApiService tagService;
        private readonly IPageApiService pageService;

        public CmsContext(ITagApiService tagService, IPageApiService pageService)
        {
            this.tagService = tagService;
            this.pageService = pageService;
        }

        public ITagApiService Tags
        {
            get
            {
                return tagService;
            }
        }

        public IPageApiService Pages
        {
            get
            {
                return pageService;
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