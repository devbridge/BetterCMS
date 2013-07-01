using BetterCms.Module.Api.Operations.MediaManager;
using BetterCms.Module.Api.Operations.Pages;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api
{
    public interface IApiFacade
    {
        IRootApiOperations Root { get; }

        IPagesApiOperations Pages { get; }
        
        IMediaManagerApiOperations Media { get; }
    }
}