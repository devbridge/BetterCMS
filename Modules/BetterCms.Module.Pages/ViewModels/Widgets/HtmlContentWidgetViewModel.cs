using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Core.Models;
using BetterCms.Module.Pages.Content.Resources;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// A widget view model.
    /// </summary>
    public class HtmlContentWidgetViewModel : WidgetViewModel
    {
        /// <summary>
        /// Gets or sets the desirable status for the saved widget.
        /// </summary>
        /// <value>
        /// The desirable status.
        /// </value>
        public ContentStatus DesirableStatus { get; set; }

        /// <summary>
        /// Gets or sets the content name.
        /// </summary>
        /// <value>
        /// The content name.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "HtmlContentWidget_ContentName_RequiredMessage")]
        [StringLength(200, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "HtmlContentWidget_ContentName_MaxLengthMessage")]
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
        public string CustomCSS { get; set; }

        public bool EnableCustomJS { get; set; }

        public string CustomJS { get; set; }       

        public override string ToString()
        {
            return string.Format("{0}, Name: {1}, EnableCustomCSS: {2}, EnableCustomJS: {3}", base.ToString(), Name, EnableCustomCSS, EnableCustomJS);
        }
    }
}