using System.Collections.Generic;
using System.Linq;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Users.ViewModels.User;
using NHibernate.Linq;

namespace BetterCms.Module.Users.Commands.User.GetUsersList
{
    public class GetUsersCommand : CommandBase, ICommand<SearchableGridOptions, IList<UserItemViewModel>>
    {
        public IList<UserItemViewModel> Execute(SearchableGridOptions gridOptions)
        {
           // var role = new List<Models.Role>().Add(new Models.Role(){ Name = "name" });
            var users = Repository.AsQueryable<Models.Users>()
                .Select(t => new UserItemViewModel()
                                 {
                                     Id = t.Id,
                                     Version = t.Version,
                                     UserName = t.UserName,
                                 });
            if (gridOptions != null)
            {
                gridOptions.SetDefaultSortingOptions("UserName");
            }


            return users.AddSortingAndPaging(gridOptions).ToFuture().ToList();
        }
    }
}