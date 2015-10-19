using System.Collections.Generic;

namespace BetterCms.Configuration
{
    public class AccessControlCollection: List<AccessControlElement>
    {
        public string DefaultAccessLevel { get; set; } = "ReadWrite";
    }
}