using System;

using BetterCms.Module.Pages.ViewModels.Page;

namespace BetterCms.Module.Blog.ViewModels.Author
{
    public class RenderBlogPostAuthorViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderBlogPostAuthorViewModel" /> class.
        /// </summary>
        /// <param name="author">The author.</param>
        public RenderBlogPostAuthorViewModel(Models.Author author = null)
        {
            if (author != null)
            {
                Id = author.Id;
                Version = author.Version;
                Name = author.Name;
                Description = author.Description;
                if (author.Image != null)
                {
                    Image = new RenderPageImageViewModel(author.Image);
                }
            }
        }

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
        /// Gets or sets the author name.
        /// </summary>
        /// <value>
        /// The author name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the author description.
        /// </summary>
        /// <value>
        /// The author description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the author image view model.
        /// </summary>
        /// <value>
        /// The author image view model.
        /// </value>
        public RenderPageImageViewModel Image { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Id: {1}, Version: {2}, Name: {3}, Description: {4}", base.ToString(), Id, Version, Name, Description);
        }
    }
}