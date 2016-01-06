using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.ViewModels.Images
{
    /// <summary>
    /// View model for image media data.
    /// </summary>
    public class ImageViewModel
    {
        public ImageViewModel()
        {
            ShouldOverride = true;
        }

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
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [StringLength(MaxLength.Text, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string Description { get; set; }

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
        /// Gets or sets the cropped image width.
        /// </summary>
        /// <value>
        /// The cropped image width.
        /// </value>
        public int CroppedWidth { get; set; }

        /// <summary>
        /// Gets or sets the cropped image height.
        /// </summary>
        /// <value>
        /// The cropped image height.
        /// </value>
        public int CroppedHeight { get; set; }

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

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        public Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets override flag.
        /// </summary>
        public bool ShouldOverride { get; set; }

        /// <summary>
        /// Gets or sets the list of categories.
        /// </summary>
        /// <value>
        /// The list of categories.
        /// </value>
        public IEnumerable<LookupKeyValue> Categories { get; set; }

        /// <summary>
        /// Gets or sets the categories filter key.
        /// </summary>
        /// <value>
        /// The categories filter key.
        /// </value>
        public string CategoriesFilterKey { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Title: {2}, Url: {3}, FileName: {4}, FileExtension: {5}", Id, Version, Title, Url, FileName, FileExtension);
        }
    }
}