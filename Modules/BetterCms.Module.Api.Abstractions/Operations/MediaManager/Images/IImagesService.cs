using BetterCms.Module.Api.Operations.MediaManager.Images.Image;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    public interface IImagesService
    {
        /// <summary>
        /// Gets the upload image service.
        /// </summary>
        IUploadImageService Upload { get; }

        /// <summary>
        /// Gets images list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetImagesResponse</c> with images list.</returns>
        GetImagesResponse Get(GetImagesRequest request);

        // NOTE: do not implement: replaces all the tags.
        // PutTagsResponse Put(PutTagsRequest request);

        /// <summary>
        /// Creates a new image.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostImagesResponse</c> with a new image id.</returns>
        PostImageResponse Post(PostImageRequest request);

        // NOTE: do not implement: drops all the images.
        // DeleteImagesResponse Delete(DeleteImagesRequest request);
    }
}
