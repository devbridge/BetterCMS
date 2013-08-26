using System;

using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Users.ViewModels.User
{
    public class UserItemViewModel : IEditableGridItem
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public string UserName { get; set; }
        
        public string FullName { get; set; }
        
        public string Email { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, UserName: {2}", Id, Version, UserName);
        }
    }
}
