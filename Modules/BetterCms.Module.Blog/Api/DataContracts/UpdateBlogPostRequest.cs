using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Mvc.Attributes;
using BetterCms.Module.Root.Api.Attributes;

namespace BetterCms.Module.Blog.Api.DataContracts
{
    public class UpdateBlogPostRequest
    {
        [EmptyGuidValidation(ErrorMessage = "Blog post Id must be set.")]
        public Guid Id { get; set; }

        [DateValidation(ErrorMessage = "Invalid Activation Date.")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "PageContent_LiveFrom_RequiredMessage")]
        public DateTime ActivationDate { get; set; }

        /// <summary>
        /// Gets or sets the date, to which page is in live.
        /// </summary>
        /// <value>
        /// The date, to which page is in live.
        /// </value>
        [DateValidation(ErrorMessage = "Invalid Expiration Date.")]
        public DateTime? ExpirationDate { get; set; }
    }
}