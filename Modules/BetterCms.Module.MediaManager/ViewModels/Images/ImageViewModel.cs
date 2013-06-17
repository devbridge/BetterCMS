using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.MediaManager.ViewModels.Images
{
    /// <summary>
    /// View model for image media data.
    /// </summary>
    public class ImageViewModel
    {
        private const string dimensionRegularExpression = "^[1-9][0-9]{0,3}$";

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
        [StringLength(MaxLength.Uri, ErrorMessageResourceType = typeof(MediaGlobalization), ErrorMessageResourceName = "ImageEditor_Dialog_ImageCaption_MaxLengthMessage")]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }
        
        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>
        /// The thumbnail  URL.
        /// </value>
        public string ThumbnailUrl { get; set; }

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
        [RegularExpression(dimensionRegularExpression, ErrorMessageResourceType = typeof(MediaGlobalization), ErrorMessageResourceName = "ImageEditor_Dialog_ImageWidth_RangeMessage")]
        public int ImageWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        /// <value>
        /// The height of the image.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(MediaGlobalization), ErrorMessageResourceName = "ImageEditor_Dialog_ImageHeight_RequiredMessage")]
        [RegularExpression(dimensionRegularExpression, ErrorMessageResourceType = typeof(MediaGlobalization), ErrorMessageResourceName = "ImageEditor_Dialog_ImageHeight_RangeMessage")]
        public int ImageHeight { get; set; }

        /// <summary>
        /// Gets or sets the original image width.
        /// </summary>
        /// <value>
        /// The original image width.
        /// </value>
        public int OriginalImageWidth { get; set; }

        /// <summary>
        /// Gets or sets the original image height.
        /// </summary>
        /// <value>
        /// The original image height.
        /// </value>
        public int OriginalImageHeight { get; set; }

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
        public int CropCoordX1 { get; set; }

        /// <summary>
        /// Gets or sets the second crop point X coordinate.
        /// </summary>
        /// <value>
        /// The crop X coordinate.
        /// </value>
        public int CropCoordX2 { get; set; }

        /// <summary>
        /// Gets or sets the first crop point Y coordinate.
        /// </summary>
        /// <value>
        /// The crop Y coordinate.
        /// </value>
        public int CropCoordY1 { get; set; }

        /// <summary>
        /// Gets or sets the second crop point Y coordinate.
        /// </summary>
        /// <value>
        /// The crop Y coordinate.
        /// </value>
        public int CropCoordY2 { get; set; }
    }
}