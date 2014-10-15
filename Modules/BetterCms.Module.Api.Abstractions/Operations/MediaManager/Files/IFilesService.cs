using BetterCms.Module.Api.Operations.MediaManager.Files.File;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    /// <summary>
    /// Image service contract for REST.
    /// </summary>
    public interface IFilesService
    {
        /// <summary>
        /// Gets the upload file service.
        /// </summary>
        IUploadFileService Upload { get; }

        /// <summary>
        /// Gets files list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetFilesResponse</c> with files list.</returns>
        GetFilesResponse Get(GetFilesRequest request);

        // NOTE: do not implement: replaces all the files.
        // PutTagsResponse Put(PutTagsRequest request);

        /// <summary>
        /// Creates a new file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostFilesResponse</c> with a new file id.</returns>
        PostFileResponse Post(PostFileRequest request);

        // NOTE: do not implement: drops all the files.
        // DeleteFilesResponse Delete(DeleteFilesRequest request);
    }
}
