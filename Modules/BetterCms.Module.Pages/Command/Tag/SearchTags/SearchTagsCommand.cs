using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Tags;
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
    public class SearchTagsCommand : CommandBase, ICommand<string, List<string>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">A filter to search for specific tags.</param>
        /// <returns>A list of tags.</returns>
        public List<string> Execute(string request)
        {
            Root.Models.Tag alias = null;
            string modelAlias = null;

            var query = UnitOfWork.Session.QueryOver(() => alias).Where(() => !alias.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request))
            {
                var searchQuery = string.Format("%{0}%", request);
                query = query.Where(Restrictions.InsensitiveLike(NHibernate.Criterion.Projections.Property(() => alias.Name), searchQuery));
            }

            var tags = query.SelectList(select => select.Select(() => alias.Name).WithAlias(() => modelAlias)).Future<string>();

            return tags.ToList();
        }
    }
}