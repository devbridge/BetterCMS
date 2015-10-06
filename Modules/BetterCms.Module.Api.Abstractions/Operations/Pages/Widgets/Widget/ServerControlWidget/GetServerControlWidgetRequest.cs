using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    [DataContract]
    [Serializable]
    public class GetServerControlWidgetRequest : RequestBase<GetServerControlWidgetModel>
    {
        [DataMember]
        public Guid WidgetId { get; set; }
    }
}