using System;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Cms;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.History.GetContentVersion
{
    /// <summary>
    /// Command for getting page content version.
    /// </summary>
    public class GetContentVersionCommand : CommandBase, ICommand<Guid, RenderPageViewModel>
    {
        /// <summary>
        /// The preview service
        /// </summary>
        private readonly IPreviewService previewService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContentVersionCommand" /> class.
        /// </summary>
        /// <param name="previewService">The preview service.</param>
        public GetContentVersionCommand(IPreviewService previewService)
        {
            this.previewService = previewService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <returns>
        /// Render page view model.
        /// </returns>
        public RenderPageViewModel Execute(Guid contentId)
        {
            return previewService.GetContentPreviewViewModel(contentId, Context.Principal, true);
        }
    }
}