using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    public class FileService : Service, IFileService
    {
        public GetFileResponse Get(GetFileRequest request)
        {
            // TODO: implement
            return new GetFileResponse
            {
                Data = new FileModel()
            };
        }
    }
}