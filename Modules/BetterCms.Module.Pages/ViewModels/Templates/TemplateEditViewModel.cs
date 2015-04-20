using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Attributes;
using BetterCms.Module.Root.Mvc.Grids;
using BetterCms.Module.Root.ViewModels.Option;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Templates
{
    public class TemplateEditViewModel :  IEditableGridItem
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
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [AllowHtml]
        [DisallowHtml(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_DisallowHtml_Field_Message")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>        
        [AllowHtml]
        [DisallowHtml(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_DisallowHtml_Field_Message")]
        [StringLength(MaxLength.Url, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string PreviewImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        /// <value>
        /// The template url.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [AllowHtml]
        [DisallowHtml(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_DisallowHtml_Field_Message")]
        [StringLength(MaxLength.Url, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        /// <value>
        /// The regions.
        /// </value>
        public IList<TemplateRegionItemViewModel> Regions { get; set; }

        /// <summary>
        /// Gets or sets the list of template options.
        /// </summary>
        /// <value>
        /// The list of template options.
        /// </value>
        public IList<OptionViewModel> Options { get; set; }

        /// <summary>
        /// Gets or sets the custom options.
        /// </summary>
        /// <value>
        /// The custom options.
        /// </value>
        public List<CustomOptionViewModel> CustomOptions { get; set; }

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