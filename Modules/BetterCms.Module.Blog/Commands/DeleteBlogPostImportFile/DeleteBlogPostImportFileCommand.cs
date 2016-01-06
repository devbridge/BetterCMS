using System;
using System.Threading.Tasks;

using BetterCms.Core.Services.Storage;

using BetterCms.Module.Blog.Services;
using BetterCms.Module.Root.Mvc;

using Common.Logging;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.DeleteBlogPostImportFile
{
    public class DeleteBlogPostImportFileCommand : CommandBase, ICommand<Guid, bool>
    {
        private readonly IBlogMLService importService;

        private readonly IStorageService storageService;

        public DeleteBlogPostImportFileCommand(IBlogMLService importService, IStorageService storageService)
        {
            this.importService = importService;
            this.storageService = storageService;
        }

        public bool Execute(Guid fileId)
        {
            var fileUri = importService.ConstructFilePath(fileId);

            Task.Factory
                    .StartNew(() => { })
                    .ContinueWith(task =>
                    {
                        try
                        {
                            storageService.RemoveObject(fileUri);
                        }
                        catch (Exception exc)
                        {
                            LogManager.GetCurrentClassLogger().Error("Failed to delete blog posts import file.", exc);
                        }
                    })
                    .ContinueWith(task =>
                    {
                        try
                        {
                            storageService.RemoveFolder(fileUri);
                        }
                        catch (Exception exc)
                        {
                            LogManager.GetCurrentClassLogger().Error("Failed to delete blog posts import folder.", exc);
                        }
                    });

            return true;
        }
    }
}