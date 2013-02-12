using System;
using System.Runtime.Serialization;

using BetterCms.Api.Interfaces.Models;
using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Projections
{
    [Serializable]
    public class PageJavaScriptProjection : IJavaScriptAccessor, ISerializable
    {
        private readonly IPage page;
        private readonly IJavaScriptAccessor jsAccessor;

        public PageJavaScriptProjection(IPage page, IJavaScriptAccessor jsAccessor)
        {
            this.page = page;
            this.jsAccessor = jsAccessor;
        }

        public PageJavaScriptProjection(SerializationInfo info, StreamingContext context)
        {
            page = (IPage)info.GetValue("page", typeof(IPage));
            jsAccessor = (IJavaScriptAccessor)info.GetValue("jsAccessor", typeof(IJavaScriptAccessor));
        }        

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("page", page, typeof(IPage));
            info.AddValue("jsAccessor", jsAccessor, jsAccessor.GetType());
        }

        /// <summary>
        /// Gets the custom java script.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>Custom JavaScript</returns>
        public string GetCustomJavaScript(System.Web.Mvc.HtmlHelper html)
        {
            return jsAccessor.GetCustomJavaScript(html);
        }
    }
}