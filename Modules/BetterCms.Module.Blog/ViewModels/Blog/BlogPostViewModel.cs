using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;

using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Pages.Mvc.Attributes;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class BlogPostViewModel
    {
        /// <summary>
        /// Gets or sets the blog post id.
        /// </summary>
        /// <value>
        /// The blog post id.
        /// </value>
        [Required]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        public virtual Guid ContentId { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        [Required]
        public virtual int Version { get; set; }

        /// <summary>
        /// Gets or sets the blog title.
        /// </summary>
        /// <value>
        /// The blog title.
        /// </value>
        [Required]
        [StringLength(MaxLength.Name)]
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the blog intro text.
        /// </summary>
        /// <value>
        /// The blog intro text.
        /// </value>
        [StringLength(MaxLength.Text)]
        public virtual string IntroText { get; set; }

        /// <summary>
        /// Gets or sets the blog content.
        /// </summary>
        /// <value>
        /// The blog content.
        /// </value>
        [AllowHtml]
        public virtual string Content { get; set; }

        /// <summary>
        /// Gets or sets the live from date.
        /// </summary>
        /// <value>
        /// The live from date.
        /// </value>
        [Required]
        public virtual DateTime LiveFromDate { get; set; }

        /// <summary>
        /// Gets or sets the live to date.
        /// </summary>
        /// <value>
        /// The live to date.
        /// </value>
        [EndDateValidation(StartDateProperty = "LiveFromDate", ErrorMessageResourceType = typeof(BlogGlobalization), ErrorMessageResourceName = "BlogPost_LiveTo_ValidationMessage")]
        public virtual DateTime? LiveToDate { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public virtual Guid? AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the cathegory.
        /// </summary>
        /// <value>
        /// The cathegory.
        /// </value>
        public virtual Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the desirable status for the saved widget.
        /// </summary>
        /// <value>
        /// The desirable status.
        /// </value>
        public ContentStatus DesirableStatus { get; set; }

        /// <summary>
        /// Gets or sets the current status.
        /// </summary>
        /// <value>
        /// The current status.
        /// </value>
        public ContentStatus CurrentStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether blog post content has published version.
        /// </summary>
        /// <value>
        /// <c>true</c> if blog post content has published version; otherwise, <c>false</c>.
        /// </value>
        public bool HasPublishedContent { get; set; }

        /// <summary>
        /// Gets or sets the post tags.
        /// </summary>
        /// <value>
        /// The post tags.
        /// </value>
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the list of authors.
        /// </summary>
        /// <value>
        /// The list of authors.
        /// </value>
        public IEnumerable<LookupKeyValue> Authors { get; set; }
        
        /// <summary>
        /// Gets or sets the list of categories.
        /// </summary>
        /// <value>
        /// The list of categories.
        /// </value>
        public IEnumerable<LookupKeyValue> Categories { get; set; }

        /// <summary>
        /// Gets or sets the image view model.
        /// </summary>
        /// <value>
        /// The image view model.
        /// </value>
        public ImageSelectorViewModel Image { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostViewModel" /> class.
        /// </summary>
        public BlogPostViewModel()
        {
            Image = new ImageSelectorViewModel();
        }
    }
}