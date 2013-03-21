using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// A widget view model.
    /// </summary>
    public class WidgetViewModel : ContentViewModel
    {        
        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        public virtual Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>        
        [StringLength(MaxLength.Url, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]      
        public virtual string PreviewImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the type of the widget.
        /// </summary>
        /// <value>
        /// The type of the widget.
        /// </value>
        public virtual WidgetType? WidgetType { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, CategoryId: {1}, PreviewImageUrl: {2}, WidgetType: {3}", base.ToString(), CategoryId, PreviewImageUrl, WidgetType);
        }
    }
}