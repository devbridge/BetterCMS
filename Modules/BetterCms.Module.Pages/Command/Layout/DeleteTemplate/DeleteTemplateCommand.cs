using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Command.Widget.DeleteWidget;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Layout.DeleteTemplate
{
    public class DeleteTemplateCommand: CommandBase, ICommand<DeleteTemplateCommandRequest, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>

        public bool Execute(DeleteTemplateCommandRequest request)
        {
            Repository.Delete<Root.Models.Layout>(request.TemplateId, request.Version);
            UnitOfWork.Commit();
            return true;
        }
    }
}