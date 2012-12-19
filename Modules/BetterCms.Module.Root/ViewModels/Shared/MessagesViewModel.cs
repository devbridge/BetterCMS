using System.Collections.Generic;

namespace BetterCms.Module.Root.ViewModels.Shared
{
    /// <summary>
    /// View model for messages partial view.
    /// </summary>
    public class MessagesViewModel
    {
        /// <summary>
        /// Gets or sets success messages.
        /// </summary>
        /// <value>
        /// Success messages.
        /// </value>
        public IEnumerable<string> Success { get; set; }

        /// <summary>
        /// Gets or sets information messages.
        /// </summary>
        /// <value>
        /// Information messages.
        /// </value>
        public IEnumerable<string> Info { get; set; }

        /// <summary>
        /// Gets or sets warning messages.
        /// </summary>
        /// <value>
        /// Warning messages.
        /// </value>
        public IEnumerable<string> Warn { get; set; }

        /// <summary>
        /// Gets or sets error messages.
        /// </summary>
        /// <value>
        /// Error messages.
        /// </value>
        public IEnumerable<string> Error { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            // TODO: cannot add any key from this object
            return string.Empty;
        }
    }
}