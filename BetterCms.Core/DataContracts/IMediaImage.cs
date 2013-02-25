using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Core.DataContracts
{
    public interface IMediaImage : IMediaFile
    {
        string Caption { get; }

        int Width { get; }

        int Height { get; }

        MediaImageAlign? ImageAlign { get; }

        int ThumbnailWidth { get; }

        int ThumbnailHeight { get; }

        long ThumbnailSize { get; }

        string PublicThumbnailUrl { get; }
    }
}
