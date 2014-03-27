using System;

namespace BetterCms.Module.Api.Operations.Root
{
    [Serializable]
    public enum AccessLevel
    {
        Deny = 1,
        Read = 2,
        ReadWrite = 3
    }
}
