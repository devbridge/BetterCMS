using BetterCms.Module.Api.Operations.MediaManager.Files;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.MediaManager.Images;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.Api.Operations.MediaManager.MediaTree;

namespace BetterCms.Module.Api.Operations.MediaManager
{
    public interface IMediaManagerApiOperations
    {
        IMediaTreeService MediaTree { get; }

        IImagesService Images { get; }

        IImageService Image { get; }

        IFilesService Files { get; }

        IFileService File { get; }
    }
}
