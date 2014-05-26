using BetterCms.Module.Api.Operations.MediaManager.Folders.Folder;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders
{
    public interface IFoldersService
    {
        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetFoldersResponse</c> with folder list.</returns>
        GetFoldersResponse Get(GetFoldersRequest request);

        // NOTE: do not implement: replaces all the folders.
        // PutFoldersResponse Put(PutFoldersRequest request);

        /// <summary>
        /// Creates a new folder.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostFolderResponse</c> with a new folder id.</returns>
        PostFolderResponse Post(PostFolderRequest request);

        // NOTE: do not implement: drops all the folders.
        // DeleteFoldersResponse Delete(DeleteFoldersRequest request);
    }
}
