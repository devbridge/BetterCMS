using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    [DataContract]
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
    }
}
