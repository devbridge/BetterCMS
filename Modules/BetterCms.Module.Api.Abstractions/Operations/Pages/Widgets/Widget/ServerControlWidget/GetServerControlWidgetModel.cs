using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    [DataContract]
    [Serializable]
    public class GetServerControlWidgetModel
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
        /// Gets or sets a value indicating whether to include widget categories.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include widget categories; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeCategories { get; set; }
    }
}
