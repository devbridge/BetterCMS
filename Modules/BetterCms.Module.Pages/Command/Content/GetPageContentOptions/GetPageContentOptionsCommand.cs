using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Pages.ViewModels.Option;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Content.GetPageContentOptions
{
    public class GetPageContentOptionsCommand : CommandBase, ICommand<Guid, PageContentOptionsViewModel>
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
        /// <param name="pageContentId">The page content id.</param>
        /// <returns></returns>        
        public PageContentOptionsViewModel Execute(Guid pageContentId)
        {
//            IList<OptionValueViewModel> options = null;
//
//            if (!pageContentId.HasDefaultValue())
//            {
//                var pageContent = Repository.AsQueryable<PageContent>()
//                                        .Where(f => f.Id == pageContentId && !f.IsDeleted && !f.Content.IsDeleted)
//                                        .Fetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
//                                        .FetchMany(f => f.Options)
//                                        .ToList()
//                                        .FirstOrDefault();
//
//                if (pageContent != null)
//                {
//                    options = new List<OptionValueViewModel>();
//
//                    if (pageContent.Options != null)
//                    {
//                        foreach (var pageContentOption in pageContent.Options.Distinct())
//                        {
//                            ContentOption contentOption = null;
//                            if (pageContent.Content.ContentOptions != null)
//                            {
//                                contentOption = pageContent.Content.ContentOptions.FirstOrDefault(f => f.Key.Trim().Equals(pageContentOption.Key.Trim(), StringComparison.OrdinalIgnoreCase));
//                            }
//
//                            options.Add(new OptionValueViewModel
//                                            {                                                
//                                                Type = pageContentOption.Type,
//                                                OptionKey = pageContentOption.Key.Trim(),
//                                                OptionValue = pageContentOption.Value,
//                                                OptionDefaultValue = contentOption != null ? contentOption.DefaultValue : null
//                                            });
//                        }
//                    }
//
//                    if (pageContent.Content.ContentOptions != null)
//                    {
//                        foreach (var contentOption in pageContent.Content.ContentOptions.Distinct())
//                        {
//                            if (!options.Any(f => f.OptionKey.Equals(contentOption.Key.Trim(), StringComparison.OrdinalIgnoreCase)))
//                            {
//                                options.Add(new OptionValueViewModel
//                                                {
//                                                    Type = contentOption.Type,                                                    
//                                                    OptionKey = contentOption.Key.Trim(),
//                                                    OptionValue = null,
//                                                    OptionDefaultValue = contentOption.DefaultValue
//                                                });
//                            }
//                        }
//                    }
//                }
//            }
//            
//            if (options == null)
//            {
//                options = new List<OptionValueViewModel>();
//            }
//
//            return new PageContentOptionsViewModel
//                       {
//                           OptionValues = options.OrderBy(o => o.OptionKey).ToList(),
//                           PageContentId = pageContentId
//                       };

            var model = new PageContentOptionsViewModel
            {
                PageContentId = pageContentId
            };

            if (!pageContentId.HasDefaultValue())
            {
                var pageContent = Repository.AsQueryable<PageContent>()
                                        .Where(f => f.Id == pageContentId && !f.IsDeleted && !f.Content.IsDeleted)
                                        .Fetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
                                        .FetchMany(f => f.Options)
                                        .ToList()
                                        .FirstOrDefault();

                if (pageContent != null)
                {
                    OptionService.SetOptionValues(model, pageContent, pageContent);
                }
            }

            return model;
        }        
    }
}