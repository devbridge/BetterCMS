using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Users.ViewModels.Role;

using NHibernate.Linq;

namespace BetterCms.Module.Users.Commands.Role.GetRoles
{
    public class GetRolesCommand : CommandBase, ICommand<SearchableGridOptions, IList<RoleItemViewModel>>
    {
        public IList<RoleItemViewModel> Execute(SearchableGridOptions gridOptions)
        {

            var roles = Repository
               .AsQueryable<Models.Role>()
               .Select(t => new RoleItemViewModel()
               {
                   Id = t.Id,
                   Version = t.Version,
                   RoleName = t.Name
               });
            if (gridOptions != null)
            {
                gridOptions.SetDefaultSortingOptions("RoleName");
            }

            return roles.AddSortingAndPaging(gridOptions).ToFuture().ToList();
        }
    }
}