using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Users
{
    [DataContract]
    public class GetUsersResponse : ListResponseBase<UserModel>
    {
    }
}