using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    public interface IDraftDestroy
    {
        /// <summary>
        /// Gets or sets a value indicating whether user can destroy draft.
        /// </summary>
        /// <value>
        /// <c>true</c> if user can destroy draft; otherwise, <c>false</c>.
        /// </value>
        bool CanDestroyDraft { get; }

        /// <summary>
        /// Gets the current status.
        /// </summary>
        /// <value>
        /// The current status.
        /// </value>
        ContentStatus CurrentStatus { get; }

        /// <summary>
        /// Gets a value indicating whether content has published content version.
        /// </summary>
        /// <value>
        /// <c>true</c> if this content published content version; otherwise, <c>false</c>.
        /// </value>
        bool HasPublishedContent { get; }
    }
}