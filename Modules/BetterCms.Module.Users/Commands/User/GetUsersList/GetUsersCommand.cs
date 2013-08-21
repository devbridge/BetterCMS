using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using BetterCms.Module.Users.ViewModels.User;

using NHibernate.Linq;

namespace BetterCms.Module.Users.Commands.User.GetUsersList
{
    public class GetUsersCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<UserItemViewModel>>
    {
        public SearchableGridViewModel<UserItemViewModel> Execute(SearchableGridOptions request)
        {
            request.SetDefaultSortingOptions("UserName");

            var query = Repository.AsQueryable<Models.Users>()
                .Select(t => new UserItemViewModel()
                                 {
                                     Id = t.Id,
                                     Version = t.Version,
                                     UserName = t.UserName,
                                 });

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(user => user.UserName.Contains(request.SearchQuery) 
                    || user.Roles.Any(role => role.Name.Contains(request.SearchQuery)));
            }

            var count = query.ToRowCountFutureValue();
            query = query.AddSortingAndPaging(request);

            return new SearchableGridViewModel<UserItemViewModel>(query.ToFuture().ToList(), request, count.Value);
        }
    }
}