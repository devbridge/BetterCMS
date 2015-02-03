using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class RenderPageCategoryViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderPageCategoryViewModel" /> class.
        /// </summary>
        public RenderPageCategoryViewModel(PageCategory category)
        {
            if (category != null)
            {
                Id = category.Id;
                Version = category.Version;
                Name = category.Category.Name;
            }
        }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the category version.
        /// </summary>
        /// <value>
        /// The category version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        /// <value>
        /// The category name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Id: {1}, Version: {2}, Name: {3}", base.ToString(), Id, Version, Name);
        }
    }
}