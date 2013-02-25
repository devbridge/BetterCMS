using System;

namespace BetterCms.Core.DataContracts.Enums
{
    /// <summary>
    /// Indicates page publishing status (auto saved for preview, draft, published, unpublished).
    /// </summary>
    [Serializable]
    public enum PageStatus
    {
        Preview = 1,
        Draft = 2,
        Published = 3,
        Unpublished = 4
    }
}
