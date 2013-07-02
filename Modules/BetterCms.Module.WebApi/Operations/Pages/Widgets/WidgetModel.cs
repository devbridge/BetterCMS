using System.Runtime.Serialization;

using BetterCms.Module.Pages.Models;

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
        [DataMember(Order = 5, Name = "widgetType")]
        public WidgetType WidgetType { get; set; }

        /// <summary>
        /// Gets or sets the widget name.
        /// </summary>
        /// <value>
        /// The widget name.
        /// </value>
        [DataMember(Order = 10, Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether widget is published.
        /// </summary>
        /// <value>
        /// <c>true</c> if widget is published; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "isPublished")]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the data widget was published on.
        /// </summary>
        /// <value>
        /// The date widget was published on.
        /// </value>
        [DataMember(Order = 30, Name = "publishedOn")]
        public System.DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the widget published user name.
        /// </summary>
        /// <value>
        /// The widget published user name.
        /// </value>
        [DataMember(Order = 40, Name = "publishedByUser")]
        public string PublishedByUser { get; set; }

        /// <summary>
        /// Gets or sets the widget category id.
        /// </summary>
        /// <value>
        /// The widget category id.
        /// </value>
        [DataMember(Order = 50, Name = "categoryId")]
        public System.Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the widget category name.
        /// </summary>
        /// <value>
        /// The widget category name.
        /// </value>
        [DataMember(Order = 60, Name = "categoryName")]
        public string CategoryName { get; set; }
    }
}