using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.WebApi.Models.Pages
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

        [DataMember(Order = 60, Name = "categoryId")]
        public Guid? CategoryId { get; set; }
    }
}