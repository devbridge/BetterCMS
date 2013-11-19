using System.Collections.Generic;

namespace BetterCms.Module.MediaManager.ViewModels.History
{
    /// <summary>
    /// Media preview model.
    /// </summary>
    public class MediaPreviewViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPreviewViewModel" /> class.
        /// </summary>
        public MediaPreviewViewModel()
        {
            Properties = new List<MediaPreviewPropertyViewModel>();
        }

        /// <summary>
        /// Gets or sets the list of preview properties.
        /// </summary>
        /// <value>
        /// The preview items.
        /// </value>
        public List<MediaPreviewPropertyViewModel> Properties { get; set; }

        /// <summary>
        /// Adds new property view model to propertie slist.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="value">The value.</param>
        /// <param name="isUrl">if set to <c>true</c> is URL.</param>
        /// <param name="isImageUrl">if set to <c>true</c> is image URL.</param>
        public void AddProperty(string title, string value, bool isUrl = false, bool isImageUrl = false)
        {
            Properties.Add(new MediaPreviewPropertyViewModel
                               {
                                   Title = title, 
                                   Value = value,
                                   IsImageUrl = isImageUrl,
                                   IsUrl = isUrl
                               });
        }

        /// <summary>
        /// Adds new image view model to propertie slist.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="value">The value.</param>
        public void AddImage(string title, string value)
        {
            AddProperty(title, value, false, true);
        }
        
        /// <summary>
        /// Adds new URL view model to propertie slist.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="value">The value.</param>
        public void AddUrl(string title, string value)
        {
            AddProperty(title, value, true);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Properties count: {0}", Properties.Count);
        }
    }
}