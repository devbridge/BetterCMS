using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
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
        /// Executes the specified request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public bool Execute(PageContentOptionsViewModel model)
        {
            if (model != null && model.WidgetOptions != null)
            {
                UnitOfWork.BeginTransaction();

                // Load content and page content as proxies
                var contentId = Repository
                    .AsQueryable<PageContent>(c => c.Id == model.PageContentId)
                    .Select(c => c.Content.Id)
                    .First();

                var content = Repository.AsProxy<ContentEntity>(contentId);
                var pageContent = Repository.AsProxy<PageContent>(model.PageContentId);

                // Load saved page options
                var keys = model.WidgetOptions
                    .Select(o => o.OptionKey.ToLowerInvariant())
                    .ToArray();

                var savedPageOptions = Repository
                    .AsQueryable<PageContentOption>(f => keys.Contains(f.ContentOption.Key.ToLower()) 
                        && f.PageContent == pageContent)
                    .Select(c => new
                                     {
                                         OptionKey = c.ContentOption.Key,
                                         PageOption = c
                                     })
                    .ToList();

                // Load all content options
                var allContentOptions = Repository
                    .AsQueryable<ContentOption>(c => c.Content == content)
                    .Select(c => new
                                     {
                                         Key = c.Key,
                                         Id = c.Id
                                     })
                    .ToList();

                // Save option changes
                foreach (var option in model.WidgetOptions)
                {
                    var savedPageOption = savedPageOptions.FirstOrDefault(o => o.OptionKey == option.OptionKey);
                    if (savedPageOption != null)
                    {
                        if (savedPageOption.PageOption.Value != option.OptionValue)
                        {
                            if (string.IsNullOrWhiteSpace(option.OptionValue))
                            {
                                Repository.Delete(savedPageOption.PageOption);
                            }
                            else
                            {
                                savedPageOption.PageOption.Value = option.OptionValue;
                                Repository.Save(savedPageOption.PageOption);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(option.OptionValue))
                        {
                            var contentOption = allContentOptions
                                .FirstOrDefault(o => o.Key.ToLowerInvariant() == option.OptionKey.ToLowerInvariant());

                            if (contentOption != null) {

                                var newOption = new PageContentOption
                                                    {
                                                        PageContent = pageContent,
                                                        ContentOption = Repository.AsProxy<ContentOption>(contentOption.Id),
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