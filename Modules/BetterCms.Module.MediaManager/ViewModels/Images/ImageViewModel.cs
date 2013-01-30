using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.ViewModels.Images
{
    /// <summary>
    /// View model for image media data.
    /// </summary>
    public class ImageViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Required]
        [StringLength(MaxLength.Name)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the original image URL.
        /// </summary>
        /// <value>
        /// The original image URL.
        /// </value>
        public string OriginalImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>
        /// The file extension.
        /// </value>
        public string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        public string FileSize { get; set; }

        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        /// <value>
        /// The width of the image.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(MediaGlobalization), ErrorMessageResourceName = "ImageEditor_Dialog_ImageWidth_RequiredMessage")]
        [Range(0, 10000, ErrorMessageResourceType = typeof(MediaGlobalization), ErrorMessageResourceName = "ImageEditor_Dialog_ImageWidth_RangeMessage")]
        public int ImageWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        /// <value>
        /// The height of the image.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(MediaGlobalization), ErrorMessageResourceName = "ImageEditor_Dialog_ImageHeight_RequiredMessage")]
        [Range(0, 10000, ErrorMessageResourceType = typeof(MediaGlobalization), ErrorMessageResourceName = "ImageEditor_Dialog_ImageHeight_RangeMessage")]
        public int ImageHeight { get; set; }

        /// <summary>
        /// Gets or sets the image align.
        /// </summary>
        /// <value>
        /// The image align.
        /// </value>
        public MediaImageAlign ImageAlign { get; set; }

        /// <summary>
        /// Gets or sets the first crop point X coordinate.
        /// </summary>
        /// <value>
        /// The crop X coordinate.
        /// </value>
        public string CropCoordX1 { get; set; }

        /// <summary>
        /// Gets or sets the second crop point X coordinate.
        /// </summary>
        /// <value>
        /// The crop X coordinate.
        /// </value>
        public string CropCoordX2 { get; set; }

        /// <summary>
        /// Gets or sets the first crop point Y coordinate.
        /// </summary>
        /// <value>
        /// The crop Y coordinate.
        /// </value>
        public string CropCoordY1 { get; set; }

        /// <summary>
        /// Gets or sets the second crop point Y coordinate.
        /// </summary>
        /// <value>
        /// The crop Y coordinate.
        /// </value>
        public string CropCoordY2 { get; set; }
    }
}