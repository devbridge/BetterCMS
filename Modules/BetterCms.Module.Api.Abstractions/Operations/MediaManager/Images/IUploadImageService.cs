using BetterCms.Module.Api.Operations.MediaManager.Images.Image;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    /// <summary>
    /// Upload image service contract.
    /// </summary>
    public interface IUploadImageService
    {
        /// <summary>
        /// Uploads a new image with a stream.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>UploadImageResponse</c> with a new image id.</returns>
        UploadImageResponse Post(UploadImageRequest request);
    }
}
