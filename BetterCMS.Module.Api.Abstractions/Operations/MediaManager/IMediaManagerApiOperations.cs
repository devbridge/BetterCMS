using BetterCms.Module.Api.Operations.MediaManager.Files;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.MediaManager.Images;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.Api.Operations.MediaManager.MediaTree;
using BetterCms.Module.Api.Operations.MediaManager.Videos;
using BetterCms.Module.Api.Operations.MediaManager.Videos.Video;

namespace BetterCms.Module.Api.Operations.MediaManager
{
    public interface IMediaManagerApiOperations
    {
        IMediaTreeService MediaTree { get; }

        IImagesService Images { get; }

        IImageService Image { get; }

        IVideosService Videos { get; }

        IVideoService Video { get; }

        IFilesService Files { get; }

        IFileService File { get; }
    }
}
