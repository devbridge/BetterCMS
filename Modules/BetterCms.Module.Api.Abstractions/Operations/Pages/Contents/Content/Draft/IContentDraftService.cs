namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.Draft
{
    public interface IContentDraftService
    {
        DestroyContentDraftResponse Delete(DestroyContentDraftRequest request);
    }
}