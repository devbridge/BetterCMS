using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.MediaManager.ViewModels;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Mvc.Attributes;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Attributes;
using BetterCms.Module.Root.ViewModels.Option;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Models;
using BetterModules.Core.Web.Models;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    /// <summary>
    /// Edit basic page properties view model.
    /// </summary>
    public class EditPagePropertiesViewModel : IAccessSecuredViewModel
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        /// <value>
        /// The name of the page.
        /// </value>
        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PageTitle_RequiredMessage")]
        [StringLength(MaxLength.Name, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PageTitle_MaxLengthMessage")]
        public string PageName { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PagePermalink_RequiredMessage")]
        [DisallowHtml(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_DisallowHtml_Field_Message")]
        [RegularExpression(PagesConstants.InternalUrlRegularExpression, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PagePermalink_InvalidMessage")]
        [StringLength(MaxLength.Url, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "EditPageProperties_PagePermalink_MaxLengthMessage")]
        public string PageUrl { get; set; }
        
        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IList<string> Tags { get; set; }
        
        /// <summary>
        /// Gets or sets the list of categories.
        /// </summary>
        /// <value>
        /// The list of categories.
        /// </value>
        public IEnumerable<LookupKeyValue> Categories { get; set; }

        /// <summary>
        /// Gets or sets the page custom CSS.
        /// </summary>
        /// <value>
        /// The page custom CSS.
        /// </value>
        [AllowHtml]
        public string PageCSS { get; set; }
        
        /// <summary>
        /// Gets or sets the page custom JavaScript.
        /// </summary>
        /// <value>
        /// The page custom JavaScript.
        /// </value>
        [AllowHtml]
        public string PageJavascript { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to create permanent redirect from old URL to new URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if create permanent redirect from old URL to new URL; otherwise, <c>false</c>.
        /// </value>
        public bool RedirectFromOldUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [update sitemap].
        /// </summary>
        /// <value>
        ///   <c>true</c> if update sitemap; otherwise, <c>false</c>.
        /// </value>
        public bool UpdateSitemap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether page is visible to everyone.
        /// </summary>
        /// <value>
        /// <c>true</c> if page is visible to everyone; otherwise, <c>false</c>.
        /// </value>
        public bool IsPagePublished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this page must not be scanned for links to follow.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this page must not be scanned for links to follow; otherwise, <c>false</c>.
        /// </value>
        public bool UseNoFollow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this page must not use the index.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this page must not use the index; otherwise, <c>false</c>.
        /// </value>
        public bool UseNoIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use canonical URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use canonical URL; otherwise, <c>false</c>.
        /// </value>
        public bool UseCanonicalUrl { get; set; }

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
        /// Gets or sets the list of languages.
        /// </summary>
        /// <value>
        /// The list of languages.
        /// </value>
        public List<LookupKeyValue> Languages { get; set; }

        /// <summary>
        /// Gets or sets the list of translation view models.
        /// </summary>
        /// <value>
        /// The list of translation view models.
        /// </value>
        public List<PageTranslationViewModel> Translations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show translations tab.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to show translations tab; otherwise, <c>false</c>.
        /// </value>
        public bool ShowTranslationsTab { get; set; }

        /// <summary>
        /// Gets or sets the translation messages.
        /// </summary>
        /// <value>
        /// The translation messages.
        /// </value>
        public UserMessages TranslationMessages { get; set; }

        /// <summary>
        /// Gets or sets the image view model.
        /// </summary>
        /// <value>
        /// The image view model.
        /// </value>
        public ImageSelectorViewModel Image { get; set; }

        /// <summary>
        /// Gets or sets the secondary image.
        /// </summary>
        /// <value>
        /// The secondary image.
        /// </value>
        public ImageSelectorViewModel SecondaryImage { get; set; }

        /// <summary>
        /// Gets or sets the featured image.
        /// </summary>
        /// <value>
        /// The featured image.
        /// </value>
        public ImageSelectorViewModel FeaturedImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in sitemap.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is in sitemap; otherwise, <c>false</c>.
        /// </value>
        public bool IsInSitemap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is archived; otherwise, <c>false</c>.
        /// </value>
        public bool IsArchived { get; set; }

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
        /// Gets or sets a value indicating whether [access control enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [access control enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool AccessControlEnabled { get; set; }

        /// <summary>
        /// Gets or sets the user access list.
        /// </summary>
        /// <value>
        /// The user access list.
        /// </value>
        public IList<UserAccessViewModel> UserAccessList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dialog should be opened in the read only mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if dialog should be opened in the read only mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current user can publish page.
        /// </summary>
        /// <value>
        /// <c>true</c> if current user can publish page; otherwise, <c>false</c>.
        /// </value>
        public bool CanPublishPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is master page.
        /// </summary>
        /// <value>
        ///   <c>true</c> if is master page; otherwise, <c>false</c>.
        /// </value>
        public bool IsMasterPage { get; set; }

        /// <summary>
        /// Gets or sets the page access protocol.
        /// </summary>
        /// <value>
        /// The page access protocol.
        /// </value>
        public ForceProtocolType ForceAccessProtocol { get; set; }

        /// <summary>
        /// Gets or sets the list of page access protocols.
        /// </summary>
        /// <value>
        /// The list of page access protocols.
        /// </value>
        public IEnumerable<LookupKeyValue> PageAccessProtocols { get; set; }

        /// <summary>
        /// Gets or sets the categories filter key.
        /// </summary>
        /// <value>
        /// The categories filter key.
        /// </value>
        public string CategoriesFilterKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditPagePropertiesViewModel" /> class.
        /// </summary>
        public EditPagePropertiesViewModel()
        {
            Image = new ImageSelectorViewModel();
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
            return string.Format("Id: {0}, Version: {1}, Name: {2}", Id, Version, PageName);
        }
    }
}