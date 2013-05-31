using System.IO;
using System.Text;
using System.Web.Hosting;

namespace BetterCms.Core.Web.DynamicHtmlLayout
{
    /// <summary>
    /// Dynamic HTML layout virtual file.
    /// </summary>
    public class DynamicHtmlLayoutVirtualFile : VirtualFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicHtmlLayoutVirtualFile" /> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        public DynamicHtmlLayoutVirtualFile(string virtualPath)
            : base(virtualPath)
        {
        }

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public override Stream Open()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes("<p>TEST DYNAMIC HTML LAYOUT START: </p> <p>@RenderSection(\"DDD\", false) @RenderBody() &nbsp;</p> <p> TEST DYNAMIC HTML LAYOUT END: </p> ");
            MemoryStream stream = new MemoryStream(byteArray);

            return stream;
        }
    }
}
