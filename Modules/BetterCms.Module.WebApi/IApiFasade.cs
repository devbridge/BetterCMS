using BetterCms.Module.Api.Operations.Pages;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api
{
    public interface IApiFasade
    {
        IRootApiOperations Root { get; }

        IPagesApiOperations Pages { get; }
    }
}