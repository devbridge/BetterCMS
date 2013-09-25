using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;

using BetterCms.Module.ImagesGallery.Content.Resources;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.ImagesGallery.ViewModels
{
    public class AlbumEditViewModel : IEditableGridItem
    {
        /// <summary>
        /// Gets or sets the author id.
        /// </summary>
        /// <value>
        /// The author id.
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}", Id, Version);
        }
    }
}