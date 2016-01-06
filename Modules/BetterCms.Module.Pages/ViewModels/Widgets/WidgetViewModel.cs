using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// A widget view model.
    /// </summary>
    public class WidgetViewModel : ContentViewModel
    {
        /// <summary>
        /// Gets or sets the list of SelectedCategories Ids.
        /// </summary>
        /// <value>
        /// The list of categories Ids.
        /// </value>
        public IList<LookupKeyValue> Categories { get; set; }

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
            return string.Format("{0}, PreviewImageUrl: {1}, WidgetType: {2}", base.ToString(), PreviewImageUrl, WidgetType);
        }
    }
}