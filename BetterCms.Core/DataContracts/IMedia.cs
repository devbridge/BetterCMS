using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Core.DataContracts
{
    public interface IMedia : IEntity
    {
        string Title { get; }

        MediaType Type { get; }

        IMediaFolder Folder { get; }
    }
}
