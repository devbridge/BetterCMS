using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.MediaManager.ViewModels
{
    public class EntityWithImageViewModel : IEditableGridItem
    {
        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        /// <value>
        /// The entity id.
        /// </value>
        [Required]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        [Required]
        public virtual int Version { get; set; }

        /// <summary>
        /// Gets or sets the image id.
        /// </summary>
        /// <value>
        /// The image id.
        /// </value>
        public virtual Guid? ImageId { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        public virtual string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>
        /// The thumbnail URL.
        /// </value>
        public virtual string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the image tooltip.
        /// </summary>
        /// <value>
        /// The image tooltip.
        /// </value>
        public virtual string ImageTooltip { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Image: {2}", Id, Version, ImageId);
        }
    }
}