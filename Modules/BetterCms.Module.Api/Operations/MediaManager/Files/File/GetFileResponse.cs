namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    public class GetFileResponse : ResponseBase<FileModel>
    {
        /// <summary>
        /// Gets or sets the list of file tags.
        /// </summary>
        /// <value>
        /// The list of file tags.
        /// </value>
        public System.Collections.Generic.IList<TagModel> Tags { get; set; }
    }
}