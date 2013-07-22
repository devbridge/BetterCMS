using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Enums
{
    /// <summary>
    /// Filtering connector types.
    /// </summary>
    [DataContract]
    public enum FilterConnector
    {
        [EnumMember]
        And,

        [EnumMember]
        Or
    }
}