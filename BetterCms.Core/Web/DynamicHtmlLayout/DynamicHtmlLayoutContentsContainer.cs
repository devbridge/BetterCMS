using System;
using System.Collections.Generic;

using BetterCms.Core.Models;

namespace BetterCms.Core.Web.DynamicHtmlLayout
{
    public static class DynamicHtmlLayoutContentsContainer
    {
        public const string DynamicHtmlLayoutVirtualPath = "BCMS_DYNAMIC_HTML_LAYOUT_VIRTUAL_PATH";

        /// <summary>
        /// The dictionary of currently processing dynamic HTML contents
        /// </summary>
        private static IDictionary<Guid, string> dynamicHtmlContents;

        /// <summary>
        /// Gets the dictionary of currently processing dynamic HTML contents.
        /// </summary>
        /// <value>
        /// The dictionary of currently processing dynamic HTML contents.
        /// </value>
        private static IDictionary<Guid, string> DynamicHtmlContents
        {
            get
            {
                if (dynamicHtmlContents == null)
                {
                    dynamicHtmlContents = new Dictionary<Guid, string>();
                }
                return dynamicHtmlContents;
            }
        }

        /// <summary>
        /// Adds the content HTML to temporary dictionary.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="html">The HTML.</param>
        public static void Push(Guid id, string html)
        {
            if (!DynamicHtmlContents.ContainsKey(id))
            {
                DynamicHtmlContents.Add(id, html);
            }
        }

        /// <summary>
        /// Pops the specified id.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        public static string Pop(string virtualPath)
        {
            var exp = virtualPath.Split('/');
            if (exp.Length > 2)
            {
                var idString = exp[exp.Length - 2];
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    return Pop(id);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the content HTML from temporary dictionary and removed form dictionary.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static string Pop(Guid id)
        {
            if (DynamicHtmlContents.ContainsKey(id))
            {
                var value = DynamicHtmlContents[id];
                DynamicHtmlContents.Remove(id);
                return value;
            }

            return null;
        }

        /// <summary>
        /// Creates the virtual path for master layout.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static string CreateMasterVirtualPath(Entity entity)
        {
            return string.Format("{0}/{1}/{2}", DynamicHtmlLayoutVirtualPath, entity.Id, entity.Version);
        }
    }
}
