using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;

using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.SuggestPages
{
    /// <summary>
    /// A command for getting list of pages by filter.
    /// </summary>
    public class SuggestPagesCommand : CommandBase, ICommand<PageSuggestionViewModel, List<PageLookupKeyValue>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A list of pages.
        /// </returns>
        public List<PageLookupKeyValue> Execute(PageSuggestionViewModel model)
        {
            var query = Repository.AsQueryable<PageProperties>()
                   .Where(page => page.Title.Contains(model.Query) || page.PageUrl.Contains(model.Query));

            if (model.ExistingItemsArray.Length > 0)
            {
                var ids = new List<Guid>();
                foreach (string idValue in model.ExistingItemsArray)
                {
                    var guid = idValue.ToGuidOrDefault();
                    if (!guid.HasDefaultValue())
                    {
                        ids.Add(guid);
                    }
                }
                if (ids.Any())
                {
                    query = query.Where(page => !ids.Contains(page.Id));
                }
            }

            if (model.ExcludedCultureId.HasValue)
            {
                var cultureProxy = Repository.AsProxy<Root.Models.Culture>(model.ExcludedCultureId.Value);
                query = query.Where(q => (q.Culture != cultureProxy || q.Culture == null));
            }
            
            var predicateBuilder = PredicateBuilder.False<PageProperties>();
            if (model.OnlyUntranslatedPages)
            {
                predicateBuilder = predicateBuilder.Or(page => page.CultureGroupIdentifier == null);
            }
            var includeIds = model.ExcplicitlyIncludedPagesArray;
            if (includeIds.Any())
            {
                predicateBuilder = predicateBuilder.Or(page => includeIds.Contains(page.Id));
            }
            query = query.Where(predicateBuilder);

            return query.OrderBy(page => page.Title)
                .Select(page => new PageLookupKeyValue
                                    {
                                        Key = page.Id.ToString().ToLowerInvariant(),
                                        Value = page.Title,
                                        CultureId = page.Culture != null ? page.Culture.Id : (Guid?) null,
                                        PageUrl = page.PageUrl
                                    })
                .ToList();
        }
    }
}