using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Pages.Mvc.Attributes;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Models;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class BlogPostViewModel : IAccessSecuredViewModel
    {
        /// <summary>
        /// Gets or sets the blog post id.
        /// </summary>
        /// <value>
        /// The blog post id.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        public virtual Guid ContentId { get; set; }

        /// <summary>
        /// Gets or sets the blog post entity version.
        /// </summary>
        /// <value>
        /// The blog post entity version.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        public virtual int Version { get; set; }

        /// <summary>
        /// Gets or sets the blog post content entity version.
        /// </summary>
        /// <value>
        /// The blog post content entity version.
        /// </value>
        public virtual int ContentVersion { get; set; }

        /// <summary>
        /// Gets or sets the blog title.
        /// </summary>
        /// <value>
        /// The blog title.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(BlogGlobalization), ErrorMessageResourceName = "BlogPost_Title_RequiredMessage")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(BlogGlobalization), ErrorMessageResourceName = "BlogPost_Title_MaxLengthMessage")]
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the meta title.
        /// </summary>
        /// <value>
        /// The meta title.
        /// </value>
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(BlogGlobalization), ErrorMessageResourceName = "BlogPost_MetaTitle_MaxLengthMessage")]
        public virtual string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the blog intro text.
        /// </summary>
        /// <value>
        /// The blog intro text.
        /// </value>
        [AllowHtml]
        [StringLength(MaxLength.Text, ErrorMessageResourceType = typeof(BlogGlobalization), ErrorMessageResourceName = "BlogPost_IntroText_MaxLengthMessage")]
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
        /// Gets or sets the blog original text.
        /// </summary>
        /// <value>
        /// The blog original text.
        /// </value>
        [AllowHtml]
        public virtual string OriginalText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether blog post content editor must be opened in source mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if blog post content editor must be opened in source mode; otherwise, <c>false</c>.
        /// </value>
        public bool EditInSourceMode { get; set; }

        /// <summary>
        /// Gets or sets the live from date.
        /// </summary>
        /// <value>
        /// The live from date.
        /// </value>
        [DateValidation(ErrorMessageResourceType = typeof(BlogGlobalization), ErrorMessageResourceName = "BlogPost_LiveFrom_DateNotValidationMessage")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(BlogGlobalization), ErrorMessageResourceName = "BlogPost_LiveFrom_RequiredMessage")]
        public virtual DateTime LiveFromDate { get; set; }

        /// <summary>
        /// Gets or sets the live to date.
        /// </summary>
        /// <value>
        /// The live to date.
        /// </value>
        [DateValidation(ErrorMessageResourceType = typeof(BlogGlobalization), ErrorMessageResourceName = "BlogPost_LiveTo_DateNotValidationMessage")]
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
        /// Gets or sets the blog URL.
        /// </summary>
        /// <value>
        /// The blog URL.
        /// </value>
        [CustomPageUrlValidation]
        [StringLength(MaxLength.Url, MinimumLength = 1, ErrorMessageResourceType = typeof(BlogGlobalization), ErrorMessageResourceName = "EditBlogPost_PagePermalink_MaxLengthMessage")]
        public string BlogUrl { get; set; }

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
        public IList<LookupKeyValue> Categories { get; set; }

        /// <summary>
        /// Gets or sets the image view model.
        /// </summary>
        /// <value>
        /// The image view model.
        /// </value>
        public ImageSelectorViewModel Image { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to create permanent redirect from old URL to new URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if create permanent redirect from old URL to new URL; otherwise, <c>false</c>.
        /// </value>
        public bool RedirectFromOldUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use canonical URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use canonical URL; otherwise, <c>false</c>.
        /// </value>
        public bool UseCanonicalUrl { get; set; }

        /// <summary>
        /// Gets or sets a value whether user confirmed content saving when affecting children pages.
        /// </summary>
        /// <value>
        /// <c>true</c> if user confirmed content saving when affecting children pages; otherwise, <c>false</c>.
        /// </value>
        public bool IsUserConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dialog should be opened in the read only mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if dialog should be opened in the read only mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Determines, if child regions should be included to the results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if child regions should be included to the results; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeChildRegions { get; set; }

        /// <summary>
        /// Gets or sets the categories filter key.
        /// </summary>
        /// <value>
        /// The categories filter key.
        /// </value>
        public string CategoriesFilterKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether content type is markdown.
        /// </summary>
        /// <value>
        /// <c>true</c> if content type is markdown; otherwise, <c>false</c>.
        /// </value>
        public ContentTextMode ContentTextMode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostViewModel" /> class.
        /// </summary>
        public BlogPostViewModel()
        {
            Image = new ImageSelectorViewModel();
            UseCanonicalUrl = true;
            ContentTextMode = ContentTextMode.Html;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Title: {2}, ContentId: {3}, ContentVersion: {4}", Id, Version, Title, ContentId, ContentVersion);
        }
    }
}