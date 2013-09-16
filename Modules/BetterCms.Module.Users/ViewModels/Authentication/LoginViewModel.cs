using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Users.Content.Resources;

namespace BetterCms.Module.Users.ViewModels.Authentication
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "User_UserName_RequiredMessage")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(UsersGlobalization), ErrorMessageResourceName = "Login_Password_RequireMessage")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public bool IsFormsAuthenticationEnabled { get; set; }

        public override string ToString()
        {
            return string.Format("UserName: {0}", UserName);
        }
    }
}