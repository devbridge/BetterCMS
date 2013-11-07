using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.Command.Layout.GetLayoutOptions
{
    public class GetLayoutOptionsCommand : CommandBase, ICommand<GetLayoutOptionsCommandRequest, IList<OptionValueEditViewModel>>
    {
        private readonly ILayoutService layoutService;

        private readonly IOptionService optionService;

        public GetLayoutOptionsCommand(ILayoutService layoutService, IOptionService optionService)
        {
            this.layoutService = layoutService;
            this.optionService = optionService;
        }

        public IList<OptionValueEditViewModel> Execute(GetLayoutOptionsCommandRequest request)
        {
            if (request.IsMasterPage)
            {
                var ids = Repository
                    .AsQueryable<Root.Models.Page>()
                    .Where(p => p.Id == request.Id)
                    .Select(p => new
                                     {
                                         MasterPageId = p.MasterPage != null ? p.MasterPage.Id : (Guid?)null,
                                         LayoutId = p.Layout != null ? p.Layout.Id : (Guid?)null
                                     })
                    .FirstOne();

                // Load master page options and set all them as parent options
                var options = optionService.GetMergedMasterPagesOptionValues(request.Id, ids.MasterPageId, ids.LayoutId);
                options.ForEach(o =>
                                    {
                                        o.UseDefaultValue = true;
                                        o.OptionDefaultValue = o.OptionValue;
                                        o.CanDeleteOption = false;
                                        o.CanEditOption = false;
                                    });

                return options;
            }

            return layoutService.GetLayoutOptionValues(request.Id);
        }
    }
}