using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser
{
    [DataContract]
    public class ValidateUserResponse : ResponseBase<bool>
    {
    }
}