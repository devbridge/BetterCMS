using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

using ContentEntity = BetterCms.Module.Root.Models.Content;

namespace BetterCms.Module.Pages.Command.Content.SavePageContentOptions
{
    public class SavePageContentOptionsCommand : CommandBase, ICommand<PageContentOptionsViewModel, bool>
    {
        /// <summary>
        /// Gets or sets the option service.
        /// </summary>
        /// <value>
        /// The option service.
        /// </value>
        public IOptionService OptionService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public bool Execute(PageContentOptionsViewModel model)
        {
            if (model != null && !model.PageContentId.HasDefaultValue() && model.OptionValues != null)
            {
                var pageContent = Repository.AsQueryable<PageContent>()
                              .Where(f => f.Id == model.PageContentId && !f.IsDeleted && !f.Content.IsDeleted)
                              .FetchMany(f => f.Options)
                              .ToList()
                              .FirstOrDefault();

                if (pageContent != null)
                {
                    UnitOfWork.BeginTransaction();

                    OptionService.SaveOptionValues(model.OptionValues, pageContent.Options, () => new PageContentOption { PageContent = pageContent });

                    UnitOfWork.Commit();   
                }                
            }

            return true;
        }
    }
}