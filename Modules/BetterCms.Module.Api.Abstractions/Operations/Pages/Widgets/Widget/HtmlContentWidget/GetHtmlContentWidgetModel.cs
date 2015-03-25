using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    [DataContract]
    [Serializable]
    public class GetHtmlContentWidgetModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include widget options.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include widget options; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include child contents options.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include child contents options; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeChildContentsOptions { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to include child contents options.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include child contents options; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeCategories { get; set; }
    }
}
