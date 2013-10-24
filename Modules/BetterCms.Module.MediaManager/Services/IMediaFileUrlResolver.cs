namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaFileUrlResolver
    {
        string GetMediaFileFullUrl(System.Guid id, string publicUrl);

        string EnsureFullPathUrl(string url);
    }
}