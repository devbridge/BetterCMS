using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    [DataContract]
    [Serializable]
    public class HtmlContentModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the content name.
        /// </summary>
        /// <value>
        /// The content name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the activation date.
        /// </summary>
        /// <value>
        /// The activation date.
        /// </value>
        [DataMember]
        public System.DateTime ActivationDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the content prerendered HTML.
        /// </summary>
        /// <value>
        /// The content prerendered HTML.
        /// </value>
        [DataMember]
        public string Html { get; set; }
        
        /// <summary>
        /// Gets or sets the content original text.
        /// </summary>
        /// <value>
        /// The content original text.
        /// </value>
        [DataMember]
        public string OriginalText { get; set; }

        /// <summary>
        /// Gets or sets the content custom CSS.
        /// </summary>
        /// <value>
        /// The content custom CSS.
        /// </value>
        [DataMember]
        public string CustomCss { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use custom CSS.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use custom CSS; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseCustomCss { get; set; }

        /// <summary>
        /// Gets or sets the content custom java script.
        /// </summary>
        /// <value>
        /// The content custom java script.
        /// </value>
        [DataMember]
        public string CustomJavaScript { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use custom JavaScript.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use custom JavaScript; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseCustomJavaScript { get; set; }

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
        /// Gets or sets the content text mode.
        /// </summary>
        /// <value>
        /// The content text mode.
        /// </value>
        [DataMember]
        public virtual ContentTextMode ContentTextMode { get; set; }
    }
}