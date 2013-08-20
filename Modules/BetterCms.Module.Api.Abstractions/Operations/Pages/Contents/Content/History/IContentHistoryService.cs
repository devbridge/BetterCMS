namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    public interface IContentHistoryService
    {
        GetContentHistoryResponse Get(GetContentHistoryRequest request);
    }
}