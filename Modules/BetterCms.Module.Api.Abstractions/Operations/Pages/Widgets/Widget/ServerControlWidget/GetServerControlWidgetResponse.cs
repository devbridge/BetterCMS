using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    [DataContract]
    public class GetServerControlWidgetResponse : ResponseBase<ServerControlWidgetModel>
    {
        /// <summary>
        /// Gets or sets the list of widget options.
        /// </summary>
        /// <value>
        /// The list of widget options.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<OptionModel> Options { get; set; }
    }
}