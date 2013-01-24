using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterCms.Module.Users.ViewModels.Role
{
    public class PermissionViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [HiddenInput(DisplayValue = true)]
        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }
}