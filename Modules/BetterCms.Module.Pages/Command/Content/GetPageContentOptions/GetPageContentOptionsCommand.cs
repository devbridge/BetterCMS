using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

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
                    model.OptionValues = OptionService.GetMergedOptionValuesForEdit(pageContent.Content.ContentOptions, pageContent.Options);
                }
            }

            return model;
        }        
    }
}