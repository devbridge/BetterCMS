using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using BetterCms.Module.Users.ViewModels.User;

namespace BetterCms.Module.Users.Commands.User.GetUsersList
{
    public class GetUsersCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<UserItemViewModel>>
    {
        public SearchableGridViewModel<UserItemViewModel> Execute(SearchableGridOptions gridOptions)
        {
            gridOptions.SetDefaultSortingOptions("UserName");

           // var role = new List<Models.Role>().Add(new Models.Role(){ Name = "name" });
            var users = Repository.AsQueryable<Models.Users>()
                .Select(t => new UserItemViewModel()
                                 {
                                     Id = t.Id,
                                     Version = t.Version,
                                     UserName = t.UserName,
                                 });

            var count = users.ToRowCountFutureValue();
            users = users.AddSortingAndPaging(gridOptions);

            return new SearchableGridViewModel<UserItemViewModel>(users.ToList(), gridOptions, count.Value);
        }
    }
}