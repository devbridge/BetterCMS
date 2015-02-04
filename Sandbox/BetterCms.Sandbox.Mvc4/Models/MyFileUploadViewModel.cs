using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Sandbox.Mvc4.Models
{
    public class MyFileUploadViewModel : RenderWidgetViewModel
    {
        public HttpPostedFileBase MyFile { get; set; }

        public string Name { get; set; }
    }
}
