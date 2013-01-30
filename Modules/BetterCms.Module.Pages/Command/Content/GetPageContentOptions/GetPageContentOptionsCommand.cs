using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

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
            IList<PageContentOptionViewModel> options = null;

            if (!pageContentId.HasDefaultValue())
            {
                var pageContent = Repository.AsQueryable<PageContent>()
                                        .Fetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
                                        .FetchMany(f => f.Options)
                                        .Where(f => f.Id == pageContentId && !f.IsDeleted && !f.Content.IsDeleted)
                                        .ToList()
                                        .FirstOrDefault();

                if (pageContent != null)
                {
                    options = new List<PageContentOptionViewModel>();

                    if (pageContent.Options != null)
                    {
                        foreach (var pageContentOption in pageContent.Options)
                        {
                            ContentOption contentOption = null;
                            if (pageContent.Content.ContentOptions != null)
                            {
                                contentOption = pageContent.Content.ContentOptions.FirstOrDefault(f => f.Key.Trim().Equals(pageContentOption.Key.Trim(), StringComparison.OrdinalIgnoreCase));
                            }
                            
                            options.Add(new PageContentOptionViewModel
                                            {                                                
                                                Type = pageContentOption.Type,
                                                OptionKey = pageContentOption.Key.Trim(),
                                                OptionValue = pageContentOption.Value,
                                                OptionDefaultValue = contentOption != null ? contentOption.DefaultValue : null
                                            });
                        }
                    }

                    if (pageContent.Content.ContentOptions != null)
                    {
                        foreach (var contentOption in pageContent.Content.ContentOptions)
                        {
                            if (!options.Any(f => f.OptionKey.Equals(contentOption.Key.Trim(), StringComparison.OrdinalIgnoreCase)))
                            {
                                options.Add(new PageContentOptionViewModel
                                                {
                                                    Type = contentOption.Type,                                                    
                                                    OptionKey = contentOption.Key.Trim(),
                                                    OptionValue = null,
                                                    OptionDefaultValue = contentOption.DefaultValue
                                                });
                            }
                        }
                    }
                }
            }
            
            if (options == null)
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