using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    [DataContract]
    public class WidgetModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the widget type.
        /// </summary>
        /// <value>
        /// The widget type.
        /// </value>
        [DataMember]
        public string WidgetType { get; set; }

        /// <summary>
        /// Gets or sets the type of the original widget.
        /// </summary>
        /// <value>
        /// The type of the original widget.
        /// </value>
        internal System.Type OriginalWidgetType { get; set; }

        /// <summary>
        /// Gets or sets the widget name.
        /// </summary>
        /// <value>
        /// The widget name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether widget is published.
        /// </summary>
        /// <value>
        /// <c>true</c> if widget is published; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the data widget was published on.
        /// </summary>
        /// <value>
        /// The date widget was published on.
        /// </value>
        [DataMember]
        public System.DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the widget published user name.
        /// </summary>
        /// <value>
        /// The widget published user name.
        /// </value>
        [DataMember]
        public string PublishedByUser { get; set; }

        /// <summary>
        /// Gets or sets the widget category id.
        /// </summary>
        /// <value>
        /// The widget category id.
        /// </value>
        [DataMember]
        public System.Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the widget category name.
        /// </summary>
        /// <value>
        /// The widget category name.
        /// </value>
        [DataMember]
        public string CategoryName { get; set; }
    }
}