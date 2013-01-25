using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Module.Users.Services
{
    public interface IRoleService
    {
        string[] GetUserRoles();
    }
}