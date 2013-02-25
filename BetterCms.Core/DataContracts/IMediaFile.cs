namespace BetterCms.Core.DataContracts
{
    public interface IMediaFile : IMedia
    {
        string PublicUrl { get; }

        long Size { get; }
    }
}
