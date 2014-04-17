using System.IO;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class UploadImportFileViewModel
    {
        public bool UseOriginalUrls { get; set; }

        public Stream FileStream { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, UseOriginalUrls: {1}", base.ToString(), UseOriginalUrls);
        }
    }
}