using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.ViewModels.File
{
    /// <summary>
    /// View model for file media data.
    /// </summary>
    public class FileViewModel : IAccessSecuredViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileViewModel"/> class.
        /// </summary>
        public FileViewModel()
        {
            Image = new ImageSelectorViewModel();
            UserAccessList = new List<UserAccessViewModel>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string Title { get; set; }

        [StringLength(MaxLength.Text, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }
        
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>
        /// The file extension.
        /// </value>
        public string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        public string FileSize { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the image view model.
        /// </summary>
        /// <value>
        /// The image view model.
        /// </value>
        public ImageSelectorViewModel Image { get; set; }

        /// <summary>
        /// Gets or sets the user access list.
        /// </summary>
        /// <value>
        /// The user access list.
        /// </value>
        public IList<UserAccessViewModel> UserAccessList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [access control enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [access control enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool AccessControlEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dialog should be opened in the read only mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if dialog should be opened in the read only mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the list of categories.
        /// </summary>
        /// <value>
        /// The list of categories.
        /// </value>
        public IEnumerable<LookupKeyValue> Categories { get; set; }

        /// <summary>
        /// Gets or sets the categories filter key.
        /// </summary>
        /// <value>
        /// The categories filter key.
        /// </value>
        public string CategoriesFilterKey { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Title: {1}, Version: {2}, FileName: {3}, FileExtension: {4}", Id, Title, Version, FileName, FileExtension);
        }
    }
}