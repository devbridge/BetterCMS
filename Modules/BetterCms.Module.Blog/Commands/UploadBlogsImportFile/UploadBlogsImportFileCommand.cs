using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Blog.Commands.UploadBlogsImportFile
{
    public class UploadBlogsImportFileCommand : CommandBase, ICommand<UploadImportFileViewModel, UploadBlogsImportFileResponse>
    {
        private readonly IBlogMLService importService;

        public UploadBlogsImportFileCommand(IBlogMLService importService)
        {
            this.importService = importService;
        }

        public UploadBlogsImportFileResponse Execute(UploadImportFileViewModel request)
        {
            var blogs = importService.DeserializeXMLStream(request.FileStream);
            var results = importService.ValidateImport(blogs, request.UseOriginalUrls);

            return new UploadBlogsImportFileResponse { Results = results };
        }
    }
}