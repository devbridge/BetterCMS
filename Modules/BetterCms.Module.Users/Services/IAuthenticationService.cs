using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Module.Users.Services
{
    public interface IAuthenticationService
    {
        string GeneratePasswordSalt();

        string CreatePasswordHash(string password, string passwordSaltBase64);
    }
}