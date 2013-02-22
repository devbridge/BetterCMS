using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [StringLength(200, ErrorMessage = "User name should not be longer than 200 characters")]
        public string UserName { get; set; }

        [StringLength(200, ErrorMessage = "First name should not be longer than 200 characters")]
        public string FirstName { get; set; }

        [StringLength(200, ErrorMessage = "Last name should not be longer than 200 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[\w_\+-]+(\.[\w_\+-]+)*@[\w-]+(\.[\w-]+)*\.([a-zA-Z]{2,4})$", ErrorMessage = "Email format is not valid")]
        [StringLength(400, ErrorMessage = "Email should not be longer than 200 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^.{4}(.{255})?$")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^.{4}(.{255})?$")]
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