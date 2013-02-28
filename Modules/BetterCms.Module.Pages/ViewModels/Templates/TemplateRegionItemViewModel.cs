using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.ViewModels.Templates
{
    public class TemplateRegionItemViewModel : IEditableGridItem, IEquatable<TemplateRegionItemViewModel> 
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [StringLength(MaxLength.Name)]
        public string Description { get; set; }
        
        [Required]
        [StringLength(MaxLength.Name)]
        public string Identifier { get; set; }

        public bool Equals(TemplateRegionItemViewModel other)
        {
            return Identifier == other.Identifier;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Description: {2}, Identifier: {3}", Id, Version, Description, Identifier);
        }
    }
}