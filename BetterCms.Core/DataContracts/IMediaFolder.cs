using System.Collections.Generic;

namespace BetterCms.Core.DataContracts
{
    public interface IMediaFolder : IMedia
    {
        IMediaFolder ParentFolder { get; }

        IEnumerable<IMedia> Medias { get; }
    }
}
