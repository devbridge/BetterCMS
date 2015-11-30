// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidgetsController.cs" company="Devbridge Group LLC">
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
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using System.Windows.Forms.VisualStyles;

using Amazon.Auth.AccessControlPolicy;

using BetterCms.Module.Api;
using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.MediaManager.Files;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Tree;
using BetterCms.Module.Root.Controllers;
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
            // LOCAL TESTING
            var res = "Form is empty!";
            if (string.IsNullOrEmpty(model.Name))
            {
                return Content(res);
            }
            using (var api = ApiFactory.Create())
            {
                var fileId = new Guid("20CB0347-E7AF-4899-92C6-A460009F5F74");
                res = "Uploaded new file";


                var categoryTreeResponse = api.Root.Category.Nodes.Get(new GetCategoryNodesRequest { CategoryTreeId = new Guid("1BA19133-A833-4127-AD2A-A43500ECE5D2") });
                var allCategoryTreeNodes = categoryTreeResponse.Data.Items;

                var file = api.Media.File.Get(new GetFileRequest { FileId = fileId });

                var putFileRequest = file.ToPutRequest();
                putFileRequest.Data.Categories = new List<Guid>();
                putFileRequest.Data.Categories.Add(allCategoryTreeNodes[0].Id);
                api.Media.File.Put(putFileRequest);

                Thread.Sleep(1000);
                file = api.Media.File.Get(new GetFileRequest { FileId = fileId });
                putFileRequest = file.ToPutRequest();
                putFileRequest.Data.Categories = new List<Guid>();
                putFileRequest.Data.Categories.Add(allCategoryTreeNodes[1].Id);
                api.Media.File.Put(putFileRequest);

            }
            return Content(res);
        }

        public virtual ActionResult ApiTestWidget(ApiTestWidgetModel model)
        {
            using (var api = ApiFactory.Create())
            {
                var response = api.Root.Categories.Get(new GetCategoryTreesRequest());
                foreach (var item in response.Data.Items)
                {
                    model.Data += item.Id + " " + item.Name + " ";
                    foreach (var guid in item.AvailableFor)
                    {
                        model.Data += guid + " ";
                    }
                    model.Data += '\n';
                }
            }
            return PartialView(model);
        }
    }
}