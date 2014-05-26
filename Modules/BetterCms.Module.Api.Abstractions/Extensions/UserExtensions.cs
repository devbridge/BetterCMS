using System.Linq;

using BetterCms.Module.Api.Operations.Users.Users;
using BetterCms.Module.Api.Operations.Users.Users.User;

namespace BetterCms.Module.Api.Extensions
{
    public static class UserExtensions
    {
        public static PostUserRequest ToPostRequest(this GetUserResponse response)
        {
            var model = MapModel(response);

            return new PostUserRequest { Data = model };
        }

        public static PutUserRequest ToPutRequest(this GetUserResponse response)
        {
            var model = MapModel(response);

            return new PutUserRequest { Data = model, Id = response.Data.Id };
        }

        private static SaveUserModel MapModel(GetUserResponse response)
        {
            var model = new SaveUserModel
                        {
                            Version = response.Data.Version,
                            FirstName = response.Data.FirstName,
                            LastName = response.Data.LastName,
                            UserName = response.Data.UserName,
                            Email = response.Data.Email,
                            ImageId = response.Data.ImageId
                        };
            if (response.Roles != null)
            {
                model.Roles = response.Roles.Select(r => r.Name).ToList();
            }

            return model;
        }
    }
}
