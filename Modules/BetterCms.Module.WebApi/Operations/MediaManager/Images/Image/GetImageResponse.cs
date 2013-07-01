namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    public class GetImageResponse : ResponseBase<ImageModel>
    {
        /// <summary>
        /// Gets or sets the list of image tags.
        /// </summary>
        /// <value>
        /// The list of image tags.
        /// </value>
        private System.Collections.Generic.IList<TagModel> Tags { get; set; }
    }
}