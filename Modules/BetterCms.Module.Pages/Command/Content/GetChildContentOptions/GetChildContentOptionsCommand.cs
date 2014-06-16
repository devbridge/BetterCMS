using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Content.GetChildContentOptions
{
    public class GetChildContentOptionsCommand : CommandBase, ICommand<Guid, ContentOptionValuesViewModel>
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
        /// <param name="childContentId">The child content id.</param>
        /// <returns></returns>        
        public ContentOptionValuesViewModel Execute(Guid childContentId)
        {
            var model = new ContentOptionValuesViewModel
            {
                OptionValuesContainerId = childContentId
            };

            if (!childContentId.HasDefaultValue())
            {
                var contentQuery = Repository.AsQueryable<ChildContent>()
                    .Where(f => f.Id == childContentId && !f.IsDeleted && !f.Child.IsDeleted)
                    .Fetch(f => f.Child).ThenFetchMany(f => f.ContentOptions).ThenFetch(f => f.CustomOption)
                    .FetchMany(f => f.Options).ThenFetch(f => f.CustomOption)
                    .AsQueryable();
                
                var childContent = contentQuery.ToList().FirstOrDefault();

                if (childContent != null)
                {
                    model.OptionValues = OptionService.GetMergedOptionValuesForEdit(childContent.Child.ContentOptions, childContent.Options);
                    model.CustomOptions = OptionService.GetCustomOptions();
                }
            }

            return model;
        }        
    }
}