using System.IO;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class UploadImportFileViewModel
    {
        public Stream FileStream { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", base.ToString());
        }
    }
}