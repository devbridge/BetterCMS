using System;

using BetterCms.Api;
using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Commands.DeleteTag
{
    /// <summary>
    /// A command to delete given tag.
    /// </summary>
    public class DeleteTagCommand : CommandBase, ICommand<DeleteTagCommandRequest, bool>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Executed command result.</returns>
        public bool Execute(DeleteTagCommandRequest request)
        {
            var tag = Repository.Delete<Tag>(request.TagId, request.Version);
            UnitOfWork.Commit();

            // Notify.
            PagesApiContext.Events.OnTagDeleted(tag);

            return true;
        }
    }
}