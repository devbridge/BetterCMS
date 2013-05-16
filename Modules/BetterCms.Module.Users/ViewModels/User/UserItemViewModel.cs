using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Users.ViewModels.User
{
    public class UserItemViewModel : IEditableGridItem
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public string UserName { get; set; }

        public IList<Models.Role> Roles { get; set; }
    }
}
