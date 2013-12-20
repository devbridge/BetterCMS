using System;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class DefaultTemplateViewModel
    {
        /// <summary>
        /// Gets or sets the template identifier.
        /// </summary>
        /// <value>
        /// The template identifier.
        /// </value>
        public virtual Guid TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the master page identifier.
        /// </summary>
        /// <value>
        /// The master page identifier.
        /// </value>
        public virtual Guid MasterPageId { get; set; }

       /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("TemplateId: {0}, MasterPageId: {1}", TemplateId, MasterPageId);
        }
    }
}