using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Content.SavePageContentOptions
{
    public class SavePageContentOptionsCommand : CommandBase, ICommand<PageContentOptionsViewModel, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public bool Execute(PageContentOptionsViewModel model)
        {
            if (model != null && model.WidgetOptions != null)
            {
                UnitOfWork.BeginTransaction();

                foreach (var option in model.WidgetOptions)
                {
                    var pageContentOption = Repository.FirstOrDefault<PageContentOption>(f => f.ContentOption.Key.ToLower() == option.OptionKey.ToLowerInvariant());

                    if (pageContentOption != null && pageContentOption.Value != option.OptionValue)
                    {
                        pageContentOption.Value = option.OptionValue;
                        Repository.Save(pageContentOption);
                    }
                }

                UnitOfWork.Commit();                
            }

            return true;
        }
    }
}