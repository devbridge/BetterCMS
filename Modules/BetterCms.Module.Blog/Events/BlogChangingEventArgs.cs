using System.Collections.Generic;
using System.ComponentModel;

using BetterCms.Module.Blog.Models.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Blog post changing event arguments.
    /// </summary>
    public class BlogChangingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets the page model before update.
        /// </summary>
        /// <value>
        /// The page model before update.
        /// </value>
        public UpdatingBlogModel BeforeUpdate { get; private set; }

        /// <summary>
        /// Gets the page model after update.
        /// </summary>
        /// <value>
        /// The page model after update.
        /// </value>
        public UpdatingBlogModel AfterUpdate { get; private set; }

        /// <summary>
        /// Gets the cancellation messages.
        /// </summary>
        /// <value>
        /// The cancellation messages.
        /// </value>
        public List<string> CancellationErrorMessages { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogChangingEventArgs" /> class.
        /// </summary>
        /// <param name="beforeUpdate">The old page.</param>
        /// <param name="afterUpdate">The new page.</param>
        public BlogChangingEventArgs(UpdatingBlogModel beforeUpdate, UpdatingBlogModel afterUpdate)
        {
            BeforeUpdate = beforeUpdate;
            AfterUpdate = afterUpdate;
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
    }
}