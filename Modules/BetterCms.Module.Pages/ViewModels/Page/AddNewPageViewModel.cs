using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Mvc.Attributes;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Attributes;
using BetterCms.Module.Root.ViewModels.Option;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Models;

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
        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "AddNewPageProperties_PageTitle_RequiredMessage")]
        [StringLength(MaxLength.Name, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "AddNewPageProperties_PageTitle_MaxLengthMessage")]
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [AllowHtml]
        [CustomPageUrlValidation]
        [DisallowHtml(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_DisallowHtml_Field_Message")]
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
        public List<TemplateViewModel> Templates { get; set; }

        /// <summary>
        /// Gets or sets the template id.
        /// </summary>
        /// <value>
        /// The template id.
        /// </value>
        public Guid? TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the master page id.
        /// </summary>
        /// <value>
        /// The master page id.
        /// </value>
        public Guid? MasterPageId { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>
        /// The language id.
        /// </value>
        public Guid? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the list of languages
        /// </summary>
        /// <value>
        /// The list of languages.
        /// </value>
        public List<LookupKeyValue> Languages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show languages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to show languages; otherwise, <c>false</c>.
        /// </value>
        public bool ShowLanguages { get; set; }

        /// <summary>
        /// Gets or sets the page option values.
        /// </summary>
        /// <value>
        /// The page option values.
        /// </value>
        public IList<OptionValueEditViewModel> OptionValues { get; set; }

        /// <summary>
        /// Gets or sets the custom options.
        /// </summary>
        /// <value>
        /// The custom options.
        /// </value>
        public List<CustomOptionViewModel> CustomOptions { get; set; }

        /// <summary>
        /// Gets or sets the user access list.
        /// </summary>
        /// <value>
        /// The user access list.
        /// </value>
        public IList<UserAccessViewModel> UserAccessList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether access control is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if access control is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool AccessControlEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to create a master page.
        /// </summary>
        /// <value>
        ///   <c>true</c> if master page creation needed; otherwise, <c>false</c>.
        /// </value>
        public bool CreateMasterPage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewPageViewModel"/> class.
        /// </summary>
        public AddNewPageViewModel()
        {
            UserAccessList = new List<UserAccessViewModel>();
        }

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