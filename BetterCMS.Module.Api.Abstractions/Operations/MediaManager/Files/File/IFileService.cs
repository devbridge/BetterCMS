namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    public interface IFileService
    {
        GetFileResponse Get(GetFileRequest request);
    }
}