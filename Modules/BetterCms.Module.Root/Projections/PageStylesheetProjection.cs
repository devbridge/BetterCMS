using System;
using System.Runtime.Serialization;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Projections
{
    [Serializable]
    public class PageStylesheetProjection : IStylesheetAccessor, ISerializable
    {
        private readonly IPage page;
        private readonly IStylesheetAccessor styleAccessor;

        public PageStylesheetProjection(IPage page, IStylesheetAccessor styleAccessor)
        {
            this.page = page;
            this.styleAccessor = styleAccessor;
        }

        public PageStylesheetProjection(SerializationInfo info, StreamingContext context)
        {
            page = (IPage)info.GetValue("page", typeof(IPage));
            styleAccessor = (IStylesheetAccessor)info.GetValue("styleAccessor", typeof(IStylesheetAccessor));
        }        

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("page", page, typeof(IPage));
            info.AddValue("styleAccessor", styleAccessor, styleAccessor.GetType());
        }

        /// <summary>
        /// Gets the custom styles.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>Custom style</returns>
        public string[] GetCustomStyles(System.Web.Mvc.HtmlHelper html)
        {
            return styleAccessor.GetCustomStyles(html);
        }

        /// <summary>
        /// Gets the styles resources.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>Array of style resources</returns>
        public string[] GetStylesResources(System.Web.Mvc.HtmlHelper html)
        {
            return styleAccessor.GetStylesResources(html);
        }
    }
}