using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetHtmlContentWidgetById
{
    [DataContract]
    public class WidgetModel : ModelBase
    {
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

        /// <summary>
        /// Gets or sets the custom CSS.
        /// </summary>
        /// <value>
        /// The custom CSS.
        /// </value>
        [DataMember(Order = 70, Name = "customCss")]
        public string CustomCss { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use custom CSS.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use custom CSS; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 80, Name = "useCustomCss")]
        public bool UseCustomCss { get; set; }

        /// <summary>
        /// Gets or sets the HTML.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        [DataMember(Order = 90, Name = "html")]
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use HTML.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use HTML; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 100, Name = "useHtml")]
        public bool UseHtml { get; set; }

        /// <summary>
        /// Gets or sets the custom JavaScripts.
        /// </summary>
        /// <value>
        /// The custom js.
        /// </value>
        [DataMember(Order = 110, Name = "customJs")]
        public string CustomJs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use custom JavaScript.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use custom JavaScript; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 120, Name = "useCustomJs")]
        public bool UseCustomJs { get; set; }
    }
}