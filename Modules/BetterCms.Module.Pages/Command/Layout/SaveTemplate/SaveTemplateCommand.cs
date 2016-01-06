using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Templates;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Layout.SaveTemplate
{
    public class SaveTemplateCommand : CommandBase, ICommand<TemplateEditViewModel, SaveTemplateResponse>
    {
        private readonly ILayoutService layoutService;

        public SaveTemplateCommand(ILayoutService layoutService)
        {
            this.layoutService = layoutService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Response with saved entity information</returns>
        public SaveTemplateResponse Execute(TemplateEditViewModel request)
        {
            var template = layoutService.SaveLayout(request);

            return new SaveTemplateResponse
                {
                    Id = template.Id,
                    TemplateName = template.Name,
                    Version = template.Version
                };
        }
    }
}