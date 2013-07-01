using BetterCms.Module.Api.Operations.Pages.Pages;
using BetterCms.Module.Api.Operations.Pages.Pages.Page;

namespace BetterCms.Module.Api.Operations.Pages
{
    public interface IPagesApiOperations
    {
        IPagesService Pages { get; }
        
        IPageService Page { get; }
    }
}
