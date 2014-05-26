using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure.Enums
{
    /// <summary>
    /// Ordering directions enum
    /// </summary>
    [DataContract]
    [Serializable]
    public enum OrderDirection
    {
        [EnumMember]
        Asc,

        [EnumMember]
        Desc
    }
}