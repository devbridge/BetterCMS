using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Mvc.Attributes;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    /// <summary>
    /// Add new page view model.
    /// </summary>
    public class AddNewPageViewModel
    {
        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>
        /// The page title.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "AddNewPageProperties_PageTitle_RequiredMessage")]
        [StringLength(300, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "AddNewPageProperties_PageTitle_MaxLengthMessage")]
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [CustomPageUrlValidation]
        [StringLength(MaxLength.Url, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "AddNewPageProperties_PagePermalink_MaxLengthMessage")]
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the parent page URL.
        /// </summary>
        /// <value>
        /// The parent page URL.
        /// </value>
        public string ParentPageUrl { get; set; }

        /// <summary>
        /// Gets or sets the templates.
        /// </summary>
        /// <value>
        /// The templates.
        /// </value>
        public IList<TemplateViewModel> Templates { get; set; }

        /// <summary>
        /// Gets or sets the template id.
        /// </summary>
        /// <value>
        /// The template id.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "AddNewPageProperties_TemplateId_RequiredMessage")]
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Title: {0}, TemplateId: {1}", PageTitle, TemplateId);
        }
    }
}