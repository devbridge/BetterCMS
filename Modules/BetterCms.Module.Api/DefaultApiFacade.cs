using BetterCms.Module.Api.Operations.Blog;
using BetterCms.Module.Api.Operations.MediaManager;
using BetterCms.Module.Api.Operations.Pages;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api
{
    public class DefaultApiFacade : IApiFacade
    {
        private readonly IRootApiOperations root;

        private readonly IPagesApiOperations pages;
        
        private readonly IMediaManagerApiOperations media;

        private readonly IBlogApiOperations blog;

        public DefaultApiFacade(IRootApiOperations root, IPagesApiOperations pages, IMediaManagerApiOperations media, IBlogApiOperations blog)
        {            
            this.root = root;
            this.pages = pages;
            this.media = media;
            this.blog = blog;
        }

        public IRootApiOperations Root
        {
            get
            {
                return root;
            }
        }

        public IPagesApiOperations Pages
        {
            get
            {
                return pages;
            }
        }

        public IMediaManagerApiOperations Media
        {
            get
            {
                return media;
            }
        }
        
        public IBlogApiOperations Blog
        {
            get
            {
                return blog;
            }
        }
    }
}