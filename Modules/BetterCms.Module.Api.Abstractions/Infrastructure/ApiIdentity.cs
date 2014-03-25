using System.Collections.Generic;

namespace BetterCms.Module.Api.Infrastructure
{
    public class ApiIdentity
    {
        public ApiIdentity()
        {
            Roles = new List<string>();
        }

        public ApiIdentity(string name) : this()
        {
            Name = name;
        }

        public ApiIdentity(string name, IEnumerable<string> roles)
            : this(name)
        {
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    Roles.Add(role);
                }
            }
        }

        public string Name { get; set; }
        
        public IList<string> Roles { get; set; }
    }
}
