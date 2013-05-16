using System;

using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Users.ViewModels
{
    public class UserViewModel : IEditableGridItem
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public string Name { get; set; }

        public string Roles { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Verson: {1}, Name: {2}, Roles: {3}", Id, Version, Name, Roles );
        }
    }
}