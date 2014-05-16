namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    public interface IFileService
    {
        /// <summary>
        /// Gets the specified file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetFileRequest</c> with an file.</returns>
        GetFileResponse Get(GetFileRequest request);

        /// <summary>
        /// Replaces the file or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutFileResponse</c> with a file id.</returns>
        PutFileResponse Put(PutFileRequest request);

        // NOTE: do not implement: should treat the addressed member as a collection in its own right and create a new entry in it.
        // PostFileResponse Post(PostFileRequest request);

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteFileResponse</c> with success status.</returns>
        DeleteFileResponse Delete(DeleteFileRequest request);
    }
}