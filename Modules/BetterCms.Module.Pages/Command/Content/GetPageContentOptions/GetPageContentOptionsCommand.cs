using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Content.GetPageContentOptions
{
    public class GetPageContentOptionsCommand : CommandBase, ICommand<Guid, PageContentOptionsViewModel>
    {        
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="pageContentId">The page content id.</param>
        /// <returns></returns>        
        public PageContentOptionsViewModel Execute(Guid pageContentId)
        {
            var contentId = Repository
                .AsQueryable<PageContent>()
                .Where(p => p.Id == pageContentId)
                .Select(s => s.Content.Id)
                .FirstOrDefault();

            IList<PageContentOptionViewModel> options;

            if (!contentId.HasDefaultValue())
            {
                // Load page options
                options = Repository
                    .AsQueryable<PageContentOption>()
                    .Where(f => f.PageContent.Id == pageContentId)
                    .Select(f => new PageContentOptionViewModel
                        {
                            Type = f.ContentOption.Type,
                            OptionKey = f.ContentOption.Key,
                            OptionDefaultValue = f.ContentOption.DefaultValue,
                            OptionValue = f.Value
                        })
                    .ToList();

                // Load all options of current widget
                var allOptions = Repository
                    .AsQueryable<ContentOption>()
                    .Where(f => f.Content.Id == contentId)
                    .Select(f => new PageContentOptionViewModel
                    {
                        Type = f.Type,
                        OptionKey = f.Key
                    })
                    .ToList();

                foreach (var option in allOptions)
                {
                    if (!options.Any(o => o.OptionKey == option.OptionKey))
                    {
                        options.Add(option);
                    }
                }
            }
            else
            {
                options = new List<PageContentOptionViewModel>();
            }

            return new PageContentOptionsViewModel
                       {
                           WidgetOptions = options.OrderBy(o => o.OptionKey).ToList(),
                           PageContentId = pageContentId
                       };
        }
    }
}