using BetterCms.Module.Api.Operations.MediaManager.Files;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.MediaManager.Images;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.Api.Operations.MediaManager.MediaTree;

namespace BetterCms.Module.Api.Operations.MediaManager
{
    public class DefaultMediaManagerApiOperations : IMediaManagerApiOperations
    {
        public DefaultMediaManagerApiOperations(IMediaTreeService mediaTree, IImagesService images, IImageService image,
            IFilesService files, IFileService file)
        {
            MediaTree = mediaTree;
            Images = images;
            Image = image;
            Files = files;
            File = file;
        }

        public IMediaTreeService MediaTree { get; private set; }

        public IImagesService Images { get; private set; }

        public IImageService Image { get; private set; }

        public IFilesService Files { get; private set; }

        public IFileService File { get; private set; }
    }
}