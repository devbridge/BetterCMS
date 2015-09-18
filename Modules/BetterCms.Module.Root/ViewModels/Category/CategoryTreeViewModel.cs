using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Attributes;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.ViewModels.Category
{
    public class CategoryTreeViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DisallowNonActiveDirectoryNameCompliant(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_ActiveDirectoryCompliant_CategoryMessage")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "CategoryTree_Dialog_Title_RequiredMessage")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the sitemap root nodes.
        /// </summary>
        /// <value>
        /// The root nodes.
        /// </value>
        public List<CategoryTreeNodeViewModel> RootNodes { get; set; }

// TODO:
//        public bool AccessControlEnabled { get; set; }
//        public IList<UserAccessViewModel> UserAccessList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dialog should be opened in the read only mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if dialog should be opened in the read only mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

// TODO:
//        public List<LookupKeyValue> Languages { get; set; }
//        public bool ShowLanguages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show macros.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to show macros; otherwise, <c>false</c>.
        /// </value>
        public bool ShowMacros { get; set; }

        /// <summary>
        /// Gets or sets the macro.
        /// </summary>
        /// <value>
        /// The macro.
        /// </value>
        [StringLength(MaxLength.Text, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string Macro { get; set; }

        public List<CategorizableItemViewModel> CategorizableItems { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Title:{2}, RootNodes count: {3}", Id, Version, Title, RootNodes != null ? RootNodes.Count.ToString(CultureInfo.InvariantCulture) : string.Empty);
        }
    }
}