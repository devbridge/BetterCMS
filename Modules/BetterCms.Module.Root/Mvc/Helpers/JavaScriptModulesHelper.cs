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
        /// <param name="ignoreJsModules">The ignore js modules.</param>
        /// <param name="useMinifiedPaths">if set to <c>true</c> use minified paths.</param>
        /// <returns>
        /// Html string of comma separated js friendly names.
        /// </returns>
        public static HtmlString RenderCommaSeparatedNamePathPairs(this IEnumerable<JavaScriptModuleViewModel> modules, string[] ignoreJsModules = null, bool useMinifiedPaths = false)
        {
            return new HtmlString(string.Join(", ", 
                modules.Where(f => ignoreJsModules == null || !ignoreJsModules.Contains(f.Name)).Select(f => string.Concat("'", f.Name, "' : '", useMinifiedPaths ? f.MinifiedPath : f.Path, "'"))));
        }
    }
}