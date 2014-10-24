using System.Web;

namespace BetterCms.Sandbox.Mvc4.Models
{
    public class TestUploadViewModel
    {
        public string Result { get; set; }
        
        public string Type { get; set; }
        
        public string Method { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}