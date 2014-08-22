using BetterCms.Module.Api.Operations.MediaManager.Files;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.MediaManager.Folders;
using BetterCms.Module.Api.Operations.MediaManager.Folders.Folder;
using BetterCms.Module.Api.Operations.MediaManager.Images;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.Api.Operations.MediaManager.MediaTree;

namespace BetterCms.Module.Api.Operations.MediaManager
{
    /// <summary>
    /// The default media manager api operations.
    /// </summary>
    public class DefaultMediaManagerApiOperations : IMediaManagerApiOperations
    {
        public DefaultMediaManagerApiOperations(IMediaTreeService mediaTree, IImagesService images, IImageService image,
            IFilesService files, IFileService file, IFoldersService folders, IFolderService folder)
        {
            MediaTree = mediaTree;
            Folders = folders;
            Folder = folder;
            Images = images;
            Image = image;
            Files = files;
            File = file;
        }

        public IMediaTreeService MediaTree { get; private set; }

        public IFoldersService Folders { get; private set; }

        public IFolderService Folder { get; private set; }

        public IImagesService Images { get; private set; }

        public IImageService Image { get; private set; }

        public IFilesService Files { get; private set; }

        public IFileService File { get; private set; }
    }
}