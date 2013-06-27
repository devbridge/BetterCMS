using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPageById
{
    [DataContract]
    public class PageModel : ModelBase
    { 
        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [DataMember(Order = 10, Name = "pageUrl")]
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember(Order = 20, Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the page status.
        /// </summary>
        /// <value>
        /// The page status.
        /// </value>
        [DataMember(Order = 30, Name = "isPublished")]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the page published date.
        /// </summary>
        /// <value>
        /// The page published date.
        /// </value>
        [DataMember(Order = 40, Name = "publishedOn")]
        public DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the page layout id.
        /// </summary>
        /// <value>
        /// The page layout id.
        /// </value>
        [DataMember(Order = 50, Name = "layoutId")]
        public Guid LayoutId { get; set; }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        [DataMember(Order = 60, Name = "categoryId")]
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the page category.
        /// </summary>
        /// <value>
        /// The name of the page category.
        /// </value>
        [DataMember(Order = 70, Name = "categoryName")]
        public string CategoryName { get; set; }
    }
}