using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

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

                var pageContent = Repository.AsProxy<PageContent>(model.PageContentId);

                var keys = model.WidgetOptions
                    .Select(o => o.OptionKey.ToLowerInvariant())
                    .ToArray();

                var savedPageOptions = Repository
                    .AsQueryable<PageContentOption>(f => keys.Contains(f.ContentOption.Key.ToLower()) 
                        && f.PageContent == pageContent)
                    .Fetch(f => f.ContentOption)
                    .Fetch(f => f.PageContent)
                    .ToList();

                IList<ContentOption> allContentOptions = null;

                // Save option changes
                foreach (var option in model.WidgetOptions)
                {
                    var savedPageOption = savedPageOptions.FirstOrDefault(o => o.ContentOption.Key == option.OptionKey);
                    if (savedPageOption != null)
                    {
                        if (savedPageOption.Value != option.OptionValue)
                        {
                            if (string.IsNullOrWhiteSpace(option.OptionValue))
                            {
                                Repository.Delete(savedPageOption);
                            }
                            else
                            {
                                savedPageOption.Value = option.OptionValue;
                                Repository.Save(savedPageOption);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(option.OptionValue))
                        {
                            // Load all available content options
                            if (allContentOptions == null)
                            {
                                allContentOptions = Repository
                                    .AsQueryable<ContentOption>(c => c.Content == pageContent.Content)
                                    .ToList();
                            }

                            var contentOption = allContentOptions.FirstOrDefault(o => o.Key.ToLowerInvariant() == option.OptionKey.ToLowerInvariant());

                            if (contentOption != null) {

                                var newOption = new PageContentOption
                                                    {
                                                        PageContent = pageContent,
                                                        ContentOption = contentOption,
                                                        Value = option.OptionValue
                                                    };
                                Repository.Save(newOption);
                            }
                        }
                    }
                }

                UnitOfWork.Commit();                
            }

            return true;
        }
    }
}