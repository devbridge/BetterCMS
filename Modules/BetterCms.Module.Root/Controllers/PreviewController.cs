using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;

using BetterCms.Module.Root.Mvc;

using Common.Logging;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Preview controller stub.
    /// TODO: add logic to render preview dynamically with corresponding image. 
    /// </summary>
    public class PreviewController : CmsControllerBase
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Returns a preview image of layout.
        /// </summary>
        /// <param name="layoutId">The layout id.</param>
        /// <returns>Image with a layout preview.</returns>
        [HttpGet]
        public ActionResult Layout(string layoutId)
        {
            
            using (Bitmap preview = new Bitmap(210, 177))
            {
                using (Graphics graphics = Graphics.FromImage(preview))
                {
                    graphics.FillRectangle(Brushes.WhiteSmoke, 0, 0, 186, 186);
                    graphics.DrawRectangle(Pens.Silver, 0, 0, 186, 186);

                    graphics.DrawString("Rendered", new Font("Arial", 10), Brushes.Black, 10, 10);
                    graphics.DrawString("Layout Preview", new Font("Arial", 7), Brushes.Black, 10, 30);
                    graphics.DrawString(layoutId.ToString(), new Font("Arial", 5), Brushes.Black, 10, 60);
                    graphics.Flush();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        preview.Save(stream, ImageFormat.Png);

                        return File(stream.GetBuffer(), "image/png");
                    }
                }
            }
        }
    }
}

