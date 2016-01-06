using System.Linq;
using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Blog.Commands.DeleteBlogPostImportFile;
using BetterCms.Module.Blog.Commands.ExportBlogPostsCommand;
using BetterCms.Module.Blog.Commands.ImportBlogPosts;
using BetterCms.Module.Blog.Commands.UploadBlogsImportFile;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Blog.ViewModels.Filter;
using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Blog.Controllers
{
    /// <summary>
    /// Blogs import / export controller.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(BlogModuleDescriptor.BlogAreaName)]
    public class BlogMLController : CmsControllerBase
    {
        /// <summary>
        /// Uploads the blog posts import file.
        /// </summary>
        /// <returns>JSON result with import form</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.PublishContent)]
        [HttpGet]
        public ActionResult UploadImportFile()
        {
            var model = new UploadImportFileViewModel();
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
        public ActionResult UploadImportFile(HttpPostedFileBase uploadFile, UploadImportFileViewModel model)
        {
            WireJson result;
            if (ModelState.IsValid && uploadFile != null)
            {
                model.FileStream = uploadFile.InputStream;

                var response = GetCommand<UploadBlogsImportFileCommand>().ExecuteCommand(model);

                result = new WireJson(response != null, response);
            }
            else
            {
                result = new WireJson(false);
            }

            if (result.Success)
            {
                result.Messages = Messages.Success.ToArray();
            }
            else
            {
                result.Messages = Messages.Error.ToArray();
            }

            return new WrappedJsonResult
                {
                    Data = result
                };
        }

        /// <summary>
        /// Starts the blog posts importer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Import results in JSON format
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.PublishContent)]
        [HttpPost]
        public ActionResult StartImport(ImportBlogPostsViewModel model)
        {
            var response = GetCommand<ImportBlogPostsCommand>().ExecuteCommand(model);

            return WireJson(response != null, response);
        }

        /// <summary>
        /// Deletes imported file.
        /// </summary>
        /// <returns>JSON result</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.PublishContent)]
        [HttpPost]
        public ActionResult DeleteUploadedFile(string fileId)
        {
            var response = GetCommand<DeleteBlogPostImportFileCommand>().ExecuteCommand(fileId.ToGuidOrDefault());

            return WireJson(response);
        }

        /// <summary>
        /// Exports blog posts, filtered by the requested filter.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Exported blog posts XML file.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.PublishContent)]
        public ActionResult Export(BlogsFilter request)
        {
            var xml = GetCommand<ExportBlogPostsCommand>().ExecuteCommand(request);

            return File(System.Text.Encoding.UTF8.GetBytes(xml), "text/xml", "blogs.xml");
        }
    }
}
