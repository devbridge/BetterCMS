using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

using Devbridge.Platform.Core.Modules.Registration;

namespace Devbridge.Platform.Core.Web.Modules.Registration
{
    public class WebModuleRegistrationContext : ModuleRegistrationContext
    {
        public WebModuleRegistrationContext(WebModuleDescriptor moduleDescriptor) : base(moduleDescriptor)
        {
            Namespaces = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Routes = new RouteCollection();
        }

        protected ICollection<string> Namespaces { get; private set; }

        internal RouteCollection Routes { get; private set; }

        public override string GetRegistrationName()
        {
            return ((WebModuleDescriptor)ModuleDescriptor).AreaName.ToLowerInvariant();
        }

        public Route MapRoute(string name, string url)
        {
            return MapRoute(name, url, null);
        }

        public Route MapRoute(string name, string url, string[] namespaces)
        {
            return MapRoute(name, url, null, namespaces);
        }


        public Route MapRoute(string name, string url, object defaults)
        {
            return MapRoute(name, url, defaults, null);
        }


        public Route MapRoute(string name, string url, object defaults, string[] namespaces)
        {
            return MapRoute(name, url, defaults, null, namespaces);
        }

        public Route MapRoute(string name, string url, object defaults, object constraints)
        {
            return MapRoute(name, url, defaults, constraints, null);
        }

        public Route MapRoute(string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if ((namespaces == null) && (this.Namespaces != null))
            {
                namespaces = Namespaces.ToArray<string>();
            }
            Route route = Routes.MapRoute(name, url, defaults, constraints, namespaces);
            route.DataTokens["area"] = ((WebModuleDescriptor)ModuleDescriptor).AreaName;
            bool flag = (namespaces == null) || (namespaces.Length == 0);
            route.DataTokens["UseNamespaceFallback"] = flag;

            return route;
        }

        public void IgnoreRoute(string url)
        {
            Routes.Ignore(url);
        }

        public void IgnoreRoute(string url, object constraints)
        {
            Routes.Ignore(url, constraints);
        }
    }
}
