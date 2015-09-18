using System;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Attributes;

namespace BetterCms.Module.Root.ViewModels.Option
{
    [Serializable]
    public class CustomOptionViewModel : ICustomOption
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DisallowHtml(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_DisallowHtml_Name_Message")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Identifier { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Title: {0}, Identifier: {1}", Title, Identifier);
        }
    }
}