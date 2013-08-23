using System;
using System.Collections.Generic;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.Command.Layout.GetLayoutOptions
{
    public class GetLayoutOptionsCommand : CommandBase, ICommand<Guid, IList<OptionValueEditViewModel>>
    {
        private readonly ILayoutService layoutService;

        public GetLayoutOptionsCommand(ILayoutService layoutService)
        {
            this.layoutService = layoutService;
        }

        public IList<OptionValueEditViewModel> Execute(Guid request)
        {
            return layoutService.GetLayoutOptionValues(request);
        }
    }
}