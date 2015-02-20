using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

using ContentEntity = BetterCms.Module.Root.Models.Content;

namespace BetterCms.Module.Pages.Command.Content.SavePageContentOptions
{
    public class SavePageContentOptionsCommand : CommandBase, ICommand<ContentOptionValuesViewModel, SavePageContentOptionsCommandResponse>
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
        public SavePageContentOptionsCommandResponse Execute(ContentOptionValuesViewModel model)
        {
            int version = 0;
            if (model != null && !model.OptionValuesContainerId.HasDefaultValue())
            {
                var pageContent = Repository.AsQueryable<PageContent>()
                              .Where(f => f.Id == model.OptionValuesContainerId && !f.IsDeleted && !f.Content.IsDeleted)
                              .FetchMany(f => f.Options)
                              .Fetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
                              .ToList()
                              .FirstOrDefault();

                if (pageContent != null)
                {
                    UnitOfWork.BeginTransaction();

                    var optionValues = pageContent.Options.Distinct();

                    pageContent.Options = OptionService.SaveOptionValues(model.OptionValues, optionValues, () => new PageContentOption { PageContent = pageContent });

                    UnitOfWork.Commit();

                    Events.PageEvents.Instance.OnPageContentConfigured(pageContent);

                    version = pageContent.Version;
                }                
            }

            return new SavePageContentOptionsCommandResponse() { PageContentVersion = version };
        }
    }
}