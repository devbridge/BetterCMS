using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Templates
{
    public class TemplateEditViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>        
        [StringLength(MaxLength.Url)]
        public string PreviewImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        /// <value>
        /// The template url.
        /// </value>
        [Required]
        [StringLength(MaxLength.Url)]
        public string Url { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Name: {2}, Preview image url: {3}, Url: {4}", Id, Version, Name, PreviewImageUrl, Url);
        }
    }
}