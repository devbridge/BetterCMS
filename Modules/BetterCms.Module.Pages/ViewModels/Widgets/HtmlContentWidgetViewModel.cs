using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Attributes;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// A widget view model.
    /// </summary>
    public class HtmlContentWidgetViewModel : EditWidgetViewModel
    {
        /// <summary>
        /// Gets or sets the content name.
        /// </summary>
        /// <value>
        /// The content name.
        /// </value>
        [DisallowNonActiveDirectoryNameCompliant(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_ActiveDirectoryCompliant_Message")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "HtmlContentWidget_ContentName_RequiredMessage")]
        [StringLength(MaxLength.Name, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "HtmlContentWidget_ContentName_MaxLengthMessage")]
        public override string Name
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the content of the page.
        /// </summary>
        /// <value>
        /// The content of the page.
        /// </value>
        [AllowHtml]
        public string PageContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable custom HTML.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to enable custom HTML; otherwise, <c>false</c>.
        /// </value>
        public bool EnableCustomHtml { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable custom CSS.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to enable custom CSS; otherwise, <c>false</c>.
        /// </value>
        public bool EnableCustomCSS { get; set; }

        /// <summary>
        /// Gets or sets the custom CSS.
        /// </summary>
        /// <value>
        /// The custom CSS.
        /// </value>
        [AllowHtml]
        public string CustomCSS { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable custom JavaScript.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to enable custom JavaScript; otherwise, <c>false</c>.
        /// </value>
        public bool EnableCustomJS { get; set; }

        /// <summary>
        /// Gets or sets the custom JavaScript.
        /// </summary>
        /// <value>
        /// The custom JavaScript.
        /// </value>
        [AllowHtml]
        public string CustomJS { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Name: {1}, EnableCustomCSS: {2}, EnableCustomJS: {3}", base.ToString(), Name, EnableCustomCSS, EnableCustomJS);
        }
    }
}