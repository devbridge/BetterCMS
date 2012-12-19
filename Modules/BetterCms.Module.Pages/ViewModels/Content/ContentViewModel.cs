using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    public class ContentViewModel
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

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Required]
        [StringLength(MaxLength.Name)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of content options.
        /// </summary>
        /// <value>
        /// The list of content options.
        /// </value>
        public IList<ContentOptionViewModel> ContentOptions { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Name: {2}", Id, Version, Name);
        }
    }
}