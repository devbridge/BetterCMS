using System;
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
            var query =
                Repository.AsQueryable<PageContentOption>()
                          .Where(f => f.PageContent.Id == pageContentId)
                          .Select(f => new PageContentOptionViewModel
                                          {                                              
                                              Type = f.ContentOption.Type,
                                              OptionKey = f.ContentOption.Key,
                                              OptionDefaultValue = f.ContentOption.DefaultValue,
                                              OptionValue = f.Value
                                          });
                            

            return new PageContentOptionsViewModel
                       {
                           WidgetOptions = query.ToList()
                       };
        }
    }
}