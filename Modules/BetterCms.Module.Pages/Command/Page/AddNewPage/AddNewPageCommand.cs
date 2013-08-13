using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.AddNewPage
{
    public class AddNewPageCommand : CommandBase, ICommand<AddNewPageCommandRequest, AddNewPageViewModel>
    {
        public ILayoutService LayoutService { get; set; }

        public AddNewPageViewModel Execute(AddNewPageCommandRequest request)
        {
            var model = new AddNewPageViewModel { ParentPageUrl = request.ParentPageUrl };
            model.Templates = LayoutService.GetLayouts();

            if (model.Templates.Count > 0)
            {
                model.Templates.ToList().ForEach(x => x.IsActive = false);
                model.Templates.First().IsActive = true;
                model.TemplateId = model.Templates.First(t => t.IsActive).TemplateId;

                model.OptionValues = LayoutService.GetLayoutOptionValues(model.TemplateId);
            }

            return model;
        }
    }
}