using System.Collections.Generic;
using System.ComponentModel;

using BetterCms.Module.Pages.Models.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    public class PagePropertiesChangingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagePropertiesChangingEventArgs" /> class.
        /// </summary>
        /// <param name="oldPage">The old page.</param>
        /// <param name="newPage">The new page.</param>
        public PagePropertiesChangingEventArgs(UpdatingPagePropertiesModel oldPage, UpdatingPagePropertiesModel newPage)
        {
            OldPage = oldPage;
            NewPage = newPage;
            CancellationErrorMessages = new List<string>();
        }

        /// <summary>
        /// Cancels the event with error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="clearOtherMessages">if set to <c>true</c> clear other messages.</param>
        public void CancelWithErrorMessage(string message, bool clearOtherMessages = false)
        {
            if (clearOtherMessages)
            {
                CancellationErrorMessages.Clear();
            }

            Cancel = true;
            CancellationErrorMessages.Add(message);
        }

        /// <summary>
        /// Gets or sets the render page view model.
        /// </summary>
        /// <value>
        /// The render page view model.
        /// </value>
        public UpdatingPagePropertiesModel OldPage { get; private set; }

        /// <summary>
        /// Gets or sets the render page view model.
        /// </summary>
        /// <value>
        /// The render page view model.
        /// </value>
        public UpdatingPagePropertiesModel NewPage { get; private set; }

        /// <summary>
        /// Gets the cancellation messages.
        /// </summary>
        /// <value>
        /// The cancellation messages.
        /// </value>
        public List<string> CancellationErrorMessages { get; private set; }
    }
}