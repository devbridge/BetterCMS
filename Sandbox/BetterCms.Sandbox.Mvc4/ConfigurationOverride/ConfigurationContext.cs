using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Sandbox.Mvc4.ConfigurationOverride
{
    /// <summary>
    /// Ambient Context for Configuration. Basically statically accessible configuration,
    /// but we can override it. Use with caution, only in places where you can't inject through constructor
    ///  http://blogs.msdn.com/b/ploeh/archive/2007/07/23/ambientcontext.aspx
    /// </summary>
    public static class ConfigurationContext
    {
        private static IMyConfiguration configuration;

        public static IMyConfiguration Current
        {
            get
            {
                if (configuration == null)
                {
                    configuration = new MyConfiguration();
                }
                return configuration;
            }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException("configuration");
                }
                configuration = value;
            }
        }

        public static void ResetToDefault()
        {
            configuration = new MyConfiguration();
        }
    }
}