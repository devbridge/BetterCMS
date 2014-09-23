using BetterCms.Module.Api.Operations.MediaManager.Images.Image;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    /// <summary>
    /// Upload file service contract.
    /// </summary>
    public interface IUploadFileService
    {
        /// <summary>
        /// Uploads a new file from the stream.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>UploadFileResponse</c> with a new file id.</returns>
        UploadFileResponse Post(UploadFileRequest request);
    }
}
