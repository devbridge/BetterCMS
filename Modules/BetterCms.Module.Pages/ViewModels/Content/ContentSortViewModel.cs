using System;
using System.ComponentModel.DataAnnotations;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    public class ContentSortViewModel
    {        
        /// <summary>
        /// Gets or sets the widget id.
        /// </summary>
        /// <value>
        /// The content id.
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
    }
}