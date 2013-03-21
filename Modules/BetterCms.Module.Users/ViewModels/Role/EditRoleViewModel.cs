using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using BetterCms.Core.Models;
using BetterCms.Module.Users.Content.Resources;

namespace BetterCms.Module.Users.ViewModels.Role
{
    public class EditRoleViewModel
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        [Required(ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "Role_RoleName_RequiredMessage")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "Role_RoleName_MaxLengthMessage")]
        public string RoleName { get; set; }

        public IList<PermissionViewModel> PermissionsList { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, RoleName: {2}", Id, Version, RoleName);
        }
    }
}