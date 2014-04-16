using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Security;

using BetterCms.Module.Blog.Commands.UploadBlogsImportFile;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Blog.Controllers
{
    /// <summary>
    /// Blogs import controller.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(BlogModuleDescriptor.BlogAreaName)]
    public class BlogImportController : CmsControllerBase
    {
        /// <summary>
        /// Uploads the blog posts import file.
        /// </summary>
        /// <returns>JSON result with import form</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.PublishContent)]
        [HttpGet]
        public ActionResult UploadImportFile()
        {
            var model = new ImportBlogPostsViewModel();
            var view = RenderView("UploadImportFile", model);

            return ComboWireJson(true, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Uploads the blog posts import file.
        /// </summary>
        /// <param name="uploadFile">The file.</param>
        /// <param name="model">The request.</param>
        /// <returns>
        /// Upload results in JSON format
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.PublishContent)]
        [HttpPost]
        public ActionResult UploadImportFile(HttpPostedFileBase uploadFile, ImportBlogPostsViewModel model)
        {
            if (ModelState.IsValid && uploadFile != null)
            {
                model.FileStream = uploadFile.InputStream;

                var response = GetCommand<UploadBlogsImportFileCommand>().ExecuteCommand(model);

                return WireJson(response != null, response);
            }

            return WireJson(false);
        }
    }
}
