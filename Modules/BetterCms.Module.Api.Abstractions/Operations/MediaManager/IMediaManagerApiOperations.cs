using BetterCms.Module.Api.Operations.MediaManager.Files;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.MediaManager.Folders;
using BetterCms.Module.Api.Operations.MediaManager.Folders.Folder;
using BetterCms.Module.Api.Operations.MediaManager.Images;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.Api.Operations.MediaManager.MediaTree;

namespace BetterCms.Module.Api.Operations.MediaManager
{
    public interface IMediaManagerApiOperations
    {
        IMediaTreeService MediaTree { get; }

        IFoldersService Folders { get; }

        IFolderService Folder { get; }

        IImagesService Images { get; }

        IImageService Image { get; }

        IFilesService Files { get; }

        IFileService File { get; }
    }
}
