using Autofac;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.Api.Operations.Blog;
using BetterCms.Module.Api.Operations.MediaManager;
using BetterCms.Module.Api.Operations.Pages;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Api.Operations.Users;

namespace BetterCms.Module.Api
{
    public class DefaultApiFacade : IApiFacade
    {
        private ILifetimeScope lifetimeScope;

        private readonly IRootApiOperations root;

        private readonly IPagesApiOperations pages;
        
        private readonly IMediaManagerApiOperations media;

        private readonly IBlogApiOperations blog;
        
        private IUsersApiOperations users;

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
        
        public IUsersApiOperations Users
        {
            get
            {
                if (users == null)
                {
                    using (var container = ContextScopeProvider.CreateChildContainer())
                    {
                        users = container.Resolve<IUsersApiOperations>();
                        if (users == null)
                        {
                            throw new CmsApiException("Users API interfaces has no implementation. Please install BetterCms.Module.Users.Api module.");
                        }
                    }
                }

                return users;
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