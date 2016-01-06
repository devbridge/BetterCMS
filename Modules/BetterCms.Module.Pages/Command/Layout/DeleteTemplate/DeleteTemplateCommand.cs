using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Layout.DeleteTemplate
{
    public class DeleteTemplateCommand: CommandBase, ICommand<DeleteTemplateCommandRequest, bool>
    {
        private readonly ILayoutService layoutService;

        public DeleteTemplateCommand(ILayoutService layoutService)
        {
            this.layoutService = layoutService;
        }

        public bool Execute(DeleteTemplateCommandRequest request)
        {
            return layoutService.DeleteLayout(request.TemplateId, request.Version);
        }
    }
}