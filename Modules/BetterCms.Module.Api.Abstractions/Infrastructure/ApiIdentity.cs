using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    [Serializable]
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

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IList<string> Roles { get; set; }
    }
}
