using System.Linq;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Content.SaveChildContentOptions
{
    public class SaveChildContentOptionsCommand : CommandBase, ICommand<ContentOptionValuesViewModel, bool>
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
        public bool Execute(ContentOptionValuesViewModel model)
        {
            if (model != null && !model.OptionValuesContainerId.HasDefaultValue())
            {
                var childContent = Repository.AsQueryable<ChildContent>()
                              .Where(f => f.Id == model.OptionValuesContainerId && !f.IsDeleted && !f.Child.IsDeleted)
                              .FetchMany(f => f.Options)
                              .Fetch(f => f.Child).ThenFetchMany(f => f.ContentOptions)
                              .ToList()
                              .FirstOrDefault();

                if (childContent != null)
                {
                    UnitOfWork.BeginTransaction();

                    var optionValues = childContent.Options.Distinct();

                    childContent.Options = OptionService.SaveOptionValues(model.OptionValues, optionValues, () => new ChildContentOption { ChildContent = childContent });

                    UnitOfWork.Commit();

                    Events.PageEvents.Instance.OnChildContentConfigured(childContent);
                }                
            }

            return true;
        }
    }
}