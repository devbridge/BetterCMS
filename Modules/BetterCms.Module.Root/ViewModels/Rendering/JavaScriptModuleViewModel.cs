using System;
using System.Collections.Generic;

using BetterCms.Module.Root.ViewModels;

namespace BetterCms.Module.Root.Models.Rendering
{
    /// <summary>
    /// JS module view model.
    /// </summary>
    public class JavaScriptModuleViewModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the 'friendly' module name to use internally.
        /// </summary>
        /// <value>
        /// The 'friendly' module name to use internally.
        /// </value>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets registered links.
        /// </summary>
        /// <value>
        /// A list of registered links.
        /// </value>
        public ProjectionsViewModel Links { get; set; }

        /// <summary>
        /// Gets or sets the globalization.
        /// </summary>
        /// <value>
        /// The globalization.
        /// </value>
        public ProjectionsViewModel Globalization { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Name: {0}, FriendlyName: {1}, Path: {2}", Name, FriendlyName, Path);
        }
    }
}