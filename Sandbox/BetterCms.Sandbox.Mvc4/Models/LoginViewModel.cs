using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace BetterCms.Sandbox.Mvc4.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public IIdentity Identity { get; set; }
    }
}