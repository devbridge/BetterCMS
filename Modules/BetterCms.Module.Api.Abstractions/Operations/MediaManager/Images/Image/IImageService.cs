namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Image service contract for REST.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Gets the specified image.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetImageRequest</c> with an image.</returns>
        GetImageResponse Get(GetImageRequest request);

        /// <summary>
        /// Replaces the image or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutImageResponse</c> with a image id.</returns>
        PutImageResponse Put(PutImageRequest request);

        // NOTE: do not implement: should treat the addressed member as a collection in its own right and create a new entry in it.
        // PostImageResponse Post(PostImageRequest request);

        /// <summary>
        /// Deletes the specified image.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteImageResponse</c> with success status.</returns>
        DeleteImageResponse Delete(DeleteImageRequest request);
    }
}