using System.Collections.Generic;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Option;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Layout.GetLayoutOptions
{
    public class GetLayoutOptionsCommand : CommandBase, ICommand<GetLayoutOptionsCommandRequest, IList<OptionValueEditViewModel>>
    {
        private readonly ILayoutService layoutService;
        
        private readonly IMasterPageService masterPageService;

        public GetLayoutOptionsCommand(ILayoutService layoutService, IMasterPageService masterPageService)
        {
            this.layoutService = layoutService;
            this.masterPageService = masterPageService;
        }

        public IList<OptionValueEditViewModel> Execute(GetLayoutOptionsCommandRequest request)
        {
            if (request.IsMasterPage)
            {
                return masterPageService.GetMasterPageOptionValues(request.Id);
            }

            return layoutService.GetLayoutOptionValues(request.Id);
        }
    }
}