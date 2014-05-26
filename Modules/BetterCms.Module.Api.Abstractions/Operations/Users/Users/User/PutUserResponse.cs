using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    /// <summary>
    /// User update response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutUserResponse : SaveResponseBase
    {
    }
}
