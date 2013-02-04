using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Users.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Image = new ImageSelectorViewModel();    
        }

        public Guid Id { get; set; }

        public int Version { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string RetypedPassword { get; set; }

        public ImageSelectorViewModel Image { get; set; }

        /// <summary>
        /// Gets or sets the list of Role.
        /// </summary>
        /// <value>
        /// The list of Role.
        /// </value>
        public IEnumerable<LookupKeyValue> Roles { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public Guid? RoleId { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, UserName: {2}", Id, Version, UserName);
        }
    }
}