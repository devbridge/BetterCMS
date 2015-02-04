using System.Web.Mvc;
using System.Windows.Forms.VisualStyles;

using BetterCms.Module.Api;
using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.MediaManager.Files;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Sandbox.Mvc4.Models;

using httpContext = System.Web.HttpContext;

namespace BetterCms.Sandbox.Mvc4.Controllers
{
    public class WidgetsController : Controller
    {
        public ActionResult TestPostAndGet(TestPostAndGetViewModel model)
        {
            return View(model);
        }

        public ActionResult TestPostAndGetWithInheritance(TestPostAndGetInheritedViewModel model)
        {
            return View(model);
        }

        [HttpGet]
        public ActionResult MyFileUploadWidget(MyFileUploadViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult MyFileUploadWidget_Upload(MyFileUploadViewModel model)
        {
            var res = "Form is empty!";
            if (string.IsNullOrEmpty(model.Name))
            {
                return Content(res);
            }
            using (var api = ApiFactory.Create())
            {
                var getFilesRequest = new GetFilesRequest();
                var getFilesResponse = api.Media.Files.Get(getFilesRequest);

                if (getFilesResponse.Data.TotalCount != 0)
                {
                    var reuploadFileRequest = new ReuploadFileRequest();

                    reuploadFileRequest.Data.Id = getFilesResponse.Data.Items[0].Id;
                    reuploadFileRequest.Data.FileStream = model.MyFile.InputStream;
                    reuploadFileRequest.Data.FileName = model.MyFile.FileName;
                    reuploadFileRequest.Data.WaitForUploadResult = false;
                    api.Media.Files.Upload.Put(reuploadFileRequest);

                    res = "Reuploaded existing file.";
                }
                else
                {
                    var uploadFileRequest = new UploadFileRequest();
                    uploadFileRequest.Data.FileStream = model.MyFile.InputStream;
                    uploadFileRequest.Data.FileName = model.MyFile.FileName;
                    uploadFileRequest.Data.WaitForUploadResult = false;
                    api.Media.Files.Upload.Post(uploadFileRequest);
                    res = "Uploaded new file";
                }
            }
            return Content(res);
        }
    }
}