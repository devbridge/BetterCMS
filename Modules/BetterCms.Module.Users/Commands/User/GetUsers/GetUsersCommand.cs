using System.Linq;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;
using BetterCms.Module.Users.ViewModels.User;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Users.Commands.User.GetUsers
{
    public class GetUsersCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<UserItemViewModel>>
    {
        public SearchableGridViewModel<UserItemViewModel> Execute(SearchableGridOptions request)
        {
            request.SetDefaultSortingOptions("UserName");

            var query = Repository
                .AsQueryable<Models.User>()
                .Select(user => new UserItemViewModel
                    {
                        Id = user.Id,
                        Version = user.Version,
                        UserName = user.UserName,
                        FullName = (user.FirstName ?? string.Empty) 
                            + (user.FirstName != null && user.LastName != null ? " " : string.Empty) 
                            + (user.LastName ?? string.Empty),
                        Email = user.Email
                    });

            // Search
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(user => user.UserName.Contains(request.SearchQuery) 
                    || user.Email.Contains(request.SearchQuery)
                    || user.FullName.Contains(request.SearchQuery));
            }

            // Total count
            var count = query.ToRowCountFutureValue();
            
            // Sorting, Paging
            query = query.AddSortingAndPaging(request);

            return new SearchableGridViewModel<UserItemViewModel>(query.ToFuture().ToList(), request, count.Value);
        }
    }
}