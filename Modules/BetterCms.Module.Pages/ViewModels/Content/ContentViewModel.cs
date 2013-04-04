using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.Root.Content.Resources;

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
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the original id.
        /// </summary>
        /// <value>
        /// The original id.
        /// </value>
        public virtual Guid OriginalId { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        public virtual int Version { get; set; }

        /// <summary>
        /// Gets or sets the original version.
        /// </summary>
        /// <value>
        /// The original version.
        /// </value>
        public virtual int OriginalVersion { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the content status.
        /// </summary>
        /// <value>
        /// The content status.
        /// </value>
        public virtual string Status { get; set; }

        /// <summary>
        /// Gets or sets the list of content options.
        /// </summary>
        /// <value>
        /// The list of content options.
        /// </value>
        public IList<ContentOptionViewModel> ContentOptions { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Name: {2} Status: {3}", Id, Version, Name, Status);
        }
    }
}