using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Blog.ViewModels.Author
{
    public class AuthorViewModel : IEditableGridItem
    {
        /// <summary>
        /// Gets or sets the author id.
        /// </summary>
        /// <value>
        /// The author id.
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
        /// Gets or sets the author name.
        /// </summary>
        /// <value>
        /// The author name.
        /// </value>
        [Required]
        [StringLength(MaxLength.Name)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the image view model.
        /// </summary>
        /// <value>
        /// The image view model.
        /// </value>
        public ImageSelectorViewModel Image { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorViewModel" /> class.
        /// </summary>
        public AuthorViewModel()
        {
            Image = new ImageSelectorViewModel();
        }
    }
}