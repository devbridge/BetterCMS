using System;

using Autofac;

using BetterCms.Module.Api.Operations.Blog;
using BetterCms.Module.Api.Operations.MediaManager;
using BetterCms.Module.Api.Operations.Pages;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api
{
    public interface IApiFacade : IDisposable
    {
        ILifetimeScope Scope { get; set; }

        IRootApiOperations Root { get; }

        IPagesApiOperations Pages { get; }
        
        IMediaManagerApiOperations Media { get; }

        IBlogApiOperations Blog { get; }
    }
}