using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Web;
using BetterCms.Module.Users.Models;

namespace BetterCms.Module.Users.Services
{
    public class DefaultRegistrationService : IRegistrationService
    {
        private readonly IRepository repository;

        private readonly IHttpContextAccessor httpContextAccessor;

        public DefaultRegistrationService(IRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.repository = repository;
        }

        public bool IsFirstUserRegistered()
        {
            return repository.AsQueryable<User>().Any();
        }

        public void NavigateToRegisterFirstUserPage()
        {
            const string url = "/" + UsersModuleDescriptor.UsersAreaName + "/register";

            var http = httpContextAccessor.GetCurrent();

            if (http != null && http.Request != null && http.Request.Url != null)
            {
                if (!http.Request.Url.PathAndQuery.Contains(url))
                {
                    http.Response.Redirect(url, true);
                }
            }
        }
    }
}
