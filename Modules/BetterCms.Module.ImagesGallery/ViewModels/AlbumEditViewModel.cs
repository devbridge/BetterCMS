using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;

using BetterCms.Module.ImagesGallery.Content.Resources;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.ImagesGallery.ViewModels
{
    public class AlbumEditViewModel : IEditableGridItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumEditViewModel" /> class.
        /// </summary>
        public AlbumEditViewModel()
        {
            Folder = new FolderSelectorViewModel();
        }

        /// <summary>
        /// Gets or sets the album id.
        /// </summary>
        /// <value>
        /// The album id.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        public virtual int Version { get; set; }

        /// <summary>
        /// Gets or sets the album title.
        /// </summary>
        /// <value>
        /// The album title.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(ImagesGalleryGlobalization), ErrorMessageResourceName = "EditAlbum_Title_RequiredMessage")]
        [StringLength(MaxLength.Name, MinimumLength = 1, ErrorMessageResourceType = typeof(ImagesGalleryGlobalization), ErrorMessageResourceName = "EditAlbum_Title_MaxLengthMessage")]
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the cover image view model.
        /// </summary>
        /// <value>
        /// The cover image view model.
        /// </value>
        public ImageSelectorViewModel CoverImage { get; set; }

        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        public FolderSelectorViewModel Folder { get; set; }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(ImagesGalleryGlobalization), ErrorMessageResourceName = "EditAlbum_Folder_RequiredMessage")]
        public string FolderTitle { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Title: {2}", Id, Version, Title);
        }
    }
}