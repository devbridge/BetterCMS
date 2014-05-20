using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Users
{
    /// <summary>
    /// User creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostUserResponse : SaveResponseBase
    {
    }
}
