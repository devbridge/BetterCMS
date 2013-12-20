using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class ClonePageWithCultureViewModel : ClonePageViewModel
    {
        /// <summary>
        /// Gets or sets the culture id.
        /// </summary>
        /// <value>
        /// The culture id.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "ClonePageWithCulture_Culture_RequiredMessage")]
        public Guid? CultureId { get; set; }

        /// <summary>
        /// Gets or sets the list of cultures.
        /// </summary>
        /// <value>
        /// The list of cultures.
        /// </value>
        public List<LookupKeyValue> Cultures { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, CultureId: {1}", base.ToString(), CultureId);
        }
    }
}