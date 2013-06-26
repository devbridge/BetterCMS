using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Tags;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Tag.GetTagList
{
    /// <summary>
    /// A command to get tag list by filter.
    /// </summary>
    public class SearchTagsCommand : CommandBase, ICommand<string, List<LookupKeyValue>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">A filter to search for specific tags.</param>
        /// <returns>A list of tags.</returns>
        public List<LookupKeyValue> Execute(string request)
        {
            return Repository.AsQueryable<Root.Models.Tag>()
                      .Where(tag => tag.Name.Contains(request))
                      .Select(tag => new LookupKeyValue() { Key = tag.Id.ToString(), Value = tag.Name })
                      .ToList();
        }
    }
}