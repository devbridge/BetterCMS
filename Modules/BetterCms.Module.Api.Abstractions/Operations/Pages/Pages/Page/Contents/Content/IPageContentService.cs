using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content.Options;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    public interface IPageContentService
    {
        IPageContentOptionsService Options { get; }

        GetPageContentResponse Get(GetPageContentRequest request);

        PutPageContentResponse Put(PutPageContentRequest request);
        
        PostPageContentResponse Post(PostPageContentRequest request);

        DeletePageContentResponse Delete(DeletePageContentRequest request);
    }
}
