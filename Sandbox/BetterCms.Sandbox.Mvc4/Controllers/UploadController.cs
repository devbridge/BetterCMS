using System.Web.Mvc;

using BetterCms.Module.Api;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Sandbox.Mvc4.Models;

namespace BetterCms.Sandbox.Mvc4.Controllers
{
    public class UploadController : Controller
    {
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(TestUploadViewModel model)
        {
            if (model != null && model.File != null)
            {
                using (var api = ApiFactory.Create())
                {
                    if (model.Type == "file")
                    {
                        var uploadRequest = new UploadFileRequest();
                        uploadRequest.Data.FileStream = model.File.InputStream;
                        uploadRequest.Data.FileName = model.File.FileName;
                        uploadRequest.Data.WaitForUploadResult = model.Method == "sync";

                        var uploadResponse = api.Media.Files.Upload.Post(uploadRequest);

                        var getRequest = new GetFileRequest { FileId = uploadResponse.Data.Value };
                        var getResponse = api.Media.File.Get(getRequest);

                        model.Result = string.Format(
                            "<h2 style='color; green'>File upload successful!</h2> File can be downloaded here: <div><a href='{0}' />{1}</a></div>",
                            getResponse.Data.FileUrl,
                            getResponse.Data.Title);
                    }
                    else
                    {
                        var uploadRequest = new UploadImageRequest();
                        uploadRequest.Data.FileStream = model.File.InputStream;
                        uploadRequest.Data.FileName = model.File.FileName;
                        uploadRequest.Data.WaitForUploadResult = model.Method == "sync";

                        var uploadResponse = api.Media.Images.Upload.Post(uploadRequest);

                        var getRequest = new GetImageRequest { ImageId = uploadResponse.Data.Value };
                        var getResponse = api.Media.Image.Get(getRequest);

                        model.Result = string.Format(
                            "<h2 style='color; green'>Image upload successful!</h2> <div><img src='{0}' alt='{1}' /></div>",
                            getResponse.Data.ImageUrl,
                            getResponse.Data.Title);
                    }
                }
            }

            return View(model);
        }
    }
}