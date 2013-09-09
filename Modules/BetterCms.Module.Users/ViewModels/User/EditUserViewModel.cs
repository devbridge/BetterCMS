using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Core.Models;

using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Mvc.Attributes;

namespace BetterCms.Module.Users.ViewModels.User
{
    public class EditUserViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditUserViewModel" /> class.
        /// </summary>
        public EditUserViewModel()
        {
            Image = new ImageSelectorViewModel();    
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_UserName_RequiredMessage")]
        [StringLength(UsersModuleConstants.UserNameMaxLength, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_UserName_MaxLengthMessage")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_FirstName_MaxLengthMessage")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_LastName_MaxLengthMessage")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Email_RequiredMessage")]
        [RegularExpression(RootModuleConstants.EmailRegularExpression, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Email_ValidMessage")]
        [StringLength(MaxLength.Email, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Email_MaxLengthMessage")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [StringLength(MaxLength.Password, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Password_MaxLengthMessage")]
        [PasswordValidation]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the retyped password.
        /// </summary>
        /// <value>
        /// The retyped password.
        /// </value>
        [Compare("Password", ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_Password_ShouldMatchMessage")]
        public string RetypedPassword { get; set; }

        /// <summary>
        /// Gets or sets the image view model.
        /// </summary>
        /// <value>
        /// The image view model.
        /// </value>
        public ImageSelectorViewModel Image { get; set; }

        /// <summary>
        /// Gets or sets the list of the roles.
        /// </summary>
        /// <value>
        /// The list of the roles.
        /// </value>
        public IList<string> Roles { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, UserName: {2}, {3}", Id, Version, UserName, base.ToString());
        }
    }
}