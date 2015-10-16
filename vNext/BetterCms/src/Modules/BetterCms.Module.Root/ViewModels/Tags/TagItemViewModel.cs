using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Attributes;
using BetterCms.Module.Root.Mvc.Grids;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.ViewModels.Tags
{
    /// <summary>
    /// View model of tag.
    /// </summary>
    public class TagItemViewModel : IEditableGridItem
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
        [AllowHtml]
        [DisallowNonActiveDirectoryNameCompliant(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_ActiveDirectoryCompliant_TagMessage")]
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string Name { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Name: {2}", Id, Version, Name);
        }
    }
}