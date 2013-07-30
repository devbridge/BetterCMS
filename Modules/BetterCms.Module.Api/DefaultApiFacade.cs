using Autofac;

using BetterCms.Module.Api.Operations.Blog;
using BetterCms.Module.Api.Operations.MediaManager;
using BetterCms.Module.Api.Operations.Pages;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api
{
    public class DefaultApiFacade : IApiFacade
    {
        private ILifetimeScope lifetimeScope;

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

        public ILifetimeScope Scope
        {
            get
            {
                return lifetimeScope;
            }
            set
            {
                lifetimeScope = value;
            }
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

        public void Dispose()
        {
            if (lifetimeScope != null)
            {
                lifetimeScope.Dispose();
                lifetimeScope = null;
            }
        }        
    }
}