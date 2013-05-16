using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Users.ViewModels.Role
{
    public class RoleItemViewModel : IEditableGridItem
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public string RoleName { get; set; }

        public IList<PermissionViewModel> PremissionsList { get; set; } 

        public override string  ToString()
        {
 	        return string.Format("Id: {0}, Version: {1}, RoleName: {2}", Id, Version, RoleName);
        }
    }
}