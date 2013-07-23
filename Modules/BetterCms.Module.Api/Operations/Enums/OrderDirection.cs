using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Enums
{
    /// <summary>
    /// Ordering directions enum
    /// </summary>
    [DataContract]
    public enum OrderDirection
    {
        [EnumMember]
        Asc,

        [EnumMember]
        Desc
    }
}