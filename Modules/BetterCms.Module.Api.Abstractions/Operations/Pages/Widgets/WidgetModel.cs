using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    [DataContract]
    [Serializable]
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
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        [DataMember]
        public List<CategoryModel> Categories { get; set; }
    }
}