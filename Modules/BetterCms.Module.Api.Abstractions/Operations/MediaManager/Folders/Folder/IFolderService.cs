namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Folder service contract for REST.
    /// </summary>
    public interface IFolderService
    {
        /// <summary>
        /// Gets the specified folder.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetFolderRequest</c> with an folder.</returns>
        GetFolderResponse Get(GetFolderRequest request);

        /// <summary>
        /// Replaces the folder or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutFolderResponse</c> with a folder id.</returns>
        PutFolderResponse Put(PutFolderRequest request);

        // NOTE: do not implement: should treat the addressed member as a collection in its own right and create a new entry in it.
        // PostFolderResponse Post(PostFolderRequest request);

        /// <summary>
        /// Deletes the specified folder.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteFolderResponse</c> with success status.</returns>
        DeleteFolderResponse Delete(DeleteFolderRequest request);
    }
}