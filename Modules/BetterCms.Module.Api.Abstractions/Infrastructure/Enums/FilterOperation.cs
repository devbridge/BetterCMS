using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure.Enums
{
    /// <summary>
    /// Filtering operations enum
    /// </summary>
    [DataContract]
    [Serializable]
    public enum FilterOperation
    {
        [EnumMember]
        Equal,

        [EnumMember]
        NotEqual,

        [EnumMember]
        Less,

        [EnumMember]
        LessOrEqual,

        [EnumMember]
        Greater,

        [EnumMember]
        GreaterOrEqual,

        [EnumMember]
        Contains,

        [EnumMember]
        NotContains,

        [EnumMember]
        StartsWith,

        [EnumMember]
        EndsWith
    }
}