using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.ViewModels.Role;

namespace BetterCms.Module.Users.Commands.Role.GetRoles
{
    public class GetRolesCommand : CommandBase, ICommand<Guid?, IList<RoleItemViewModel>>
    {
        public IList<RoleItemViewModel> Execute(Guid? request)
        {
            var roles = Repository
               .AsQueryable<Models.Role>()
               .Select(t => new RoleItemViewModel()
               {
                   Id = t.Id,
                   Version = t.Version,
                   RoleName = t.Name
               })
               .ToList();
            return roles;
        }
    }
}