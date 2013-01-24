using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Models;
using BetterCms.Module.Users.ViewModels.Role;

using NHibernate.Transform;

namespace BetterCms.Module.Users.Commands.Role.GetPremissions
{
    public class GetPremissionsCommand : CommandBase, ICommand<Guid?, IList<PermissionViewModel>>
    {
        public IList<PermissionViewModel> Execute(Guid? request)
        {
            var premissions = Repository
                .AsQueryable<Permission>()
                .Select(t => new PermissionViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToList();

            return premissions;
        }
    }
}