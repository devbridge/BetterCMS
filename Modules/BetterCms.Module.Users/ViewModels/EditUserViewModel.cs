using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Users.Content.Resources;

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

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_UserName_RequiredMessage")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_UserName_MaxLengthMessage")]
        public string UserName { get; set; }

        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_FirstName_MaxLengthMessage")]
        public string FirstName { get; set; }

        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_LastName_MaxLengthMessage")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Email_RequiredMessage")]
        [RegularExpression(RootModuleConstants.EmailRegularExpression, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Email_ValidMessage")]
        [StringLength(MaxLength.Email, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Email_MaxLengthMessage")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Password_RequiredMessage")]
        [RegularExpression(UsersModuleConstants.PasswordRegularExpression, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Password_LengthMessage")]
        [StringLength(MaxLength.Password, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Password_MaxLengthMessage")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_RetypePassword_RequiredMessage")]
        [RegularExpression(UsersModuleConstants.PasswordRegularExpression, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Password_LengthMessage")]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_RetypePassword_EqualMessage")]
        [StringLength(MaxLength.Password, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Password_MaxLengthMessage")]
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