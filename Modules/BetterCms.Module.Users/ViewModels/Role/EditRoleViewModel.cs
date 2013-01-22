using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Module.Users.ViewModels.Role
{
    public class EditRoleViewModel
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public string RoleName { get; set; }

        public IList<Premission> PremissionsList { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, RoleName: {2}", Id, Version, RoleName);
        }
    }
}