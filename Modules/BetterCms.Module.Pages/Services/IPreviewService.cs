using System;
using System.Security.Principal;

using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Pages.Services
{
    public interface IPreviewService
    {
        /// <summary>
        /// Gets the view model for rendering widget preview.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <param name="user">The user.</param>
        /// <param name="allowJavaScript">if set to <c>true</c> [allow java script].</param>
        /// <returns>
        /// View model for rendering widget preview
        /// </returns>
        RenderPageViewModel GetContentPreviewViewModel(Guid contentId, IPrincipal user, bool allowJavaScript);
    }
}