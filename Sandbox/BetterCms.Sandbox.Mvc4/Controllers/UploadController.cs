// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadController.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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