using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Module.Root.Models.Rendering;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    /// <summary>
    /// Java script module helper methods.
    /// </summary>
    public static class JavaScriptModulesHelper
    {
        /// <summary>
        /// Renders the comma separated names.
        /// </summary>
        /// <param name="modules">The modules.</param>
        /// <returns>Html string of comma separated names.</returns>
        public static HtmlString RenderCommaSeparatedNames(this IEnumerable<JavaScriptModuleViewModel> modules)
        {
            return new HtmlString(string.Join(", ", modules.Select(f => string.Concat("'", f.Name, "'"))));
        }

        /// <summary>
        /// Renders the comma separated friendly names.
        /// </summary>
        /// <param name="modules">The modules.</param>
        /// <returns>Html string of comma separated js friendly names.</returns>
        public static HtmlString RenderCommaSeparatedFriendlyNames(this IEnumerable<JavaScriptModuleViewModel> modules)
        {
            return new HtmlString(string.Join(", ", modules.Select(f => f.FriendlyName)));
        }

        /// <summary>
        /// Renders the comma separated name and path pairs.
        /// </summary>
        /// <param name="modules">The modules.</param>
        /// <returns>Html string of comma separated js friendly names.</returns>
        public static HtmlString RenderCommaSeparatedNamePathPairs(this IEnumerable<JavaScriptModuleViewModel> modules, string forcedPath = null, params string [] ignoreForcedPathForModules)
        {
            return new HtmlString(string.Join(", ", 
                modules.Select(f => string.Concat("'", f.Name, "' : '", forcedPath != null && (ignoreForcedPathForModules == null || !ignoreForcedPathForModules.Contains(f.Name)) ? forcedPath : f.Path, "'"))));
        }
    }
}