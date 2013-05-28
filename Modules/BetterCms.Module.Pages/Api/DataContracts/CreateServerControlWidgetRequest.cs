using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.Pages.Api.DataContracts.Models;
using BetterCms.Module.Pages.Mvc.Attributes;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class CreateServerControlWidgetRequest
    {
        /// <summary>
        /// Gets or sets the widget name.
        /// </summary>
        /// <value>
        /// The widget name.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Widget name is required.")]
        [StringLength(MaxLength.Name, ErrorMessage = "Maximum length of widget name cannot exceed {1} symbols.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the widget (usually .cshtml) file path.
        /// </summary>
        /// <value>
        /// The widget (usually .cshtml) file path.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Widget path is required.")]
        [StringLength(MaxLength.Url, ErrorMessage = "Maximum length of widget path cannot exceed {1} symbols.")]
        [ValidVirtualPathValidation(ErrorMessage = "Widget by given path {0} doesn't exist.")]
        public string WidgetPath { get; set; }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the widget preview URL.
        /// </summary>
        /// <value>
        /// The widget preview URL.
        /// </value>
        public string PreviewUrl { get; set; }

        /// <summary>
        /// Gets or sets the list of widget options.
        /// </summary>
        /// <value>
        /// The list of widget options.
        /// </value>
        public IList<ContentOptionDto> Options { get; set; }
    }
}