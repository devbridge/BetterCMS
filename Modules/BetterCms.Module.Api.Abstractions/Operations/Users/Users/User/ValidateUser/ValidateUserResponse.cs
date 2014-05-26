using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser
{
    [DataContract]
    [Serializable]
    public class ValidateUserResponse : ResponseBase<ValidUserModel>
    {
    }
}