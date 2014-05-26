using BetterCms.Module.Api.Operations.Users.Roles;
using BetterCms.Module.Api.Operations.Users.Roles.Role;

namespace BetterCms.Module.Api.Extensions
{
    public static class RoleExtensions
    {
        public static PostRoleRequest ToPostRequest(this GetRoleResponse response)
        {
            var model = MapModel(response);

            return new PostRoleRequest { Data = model };
        }

        public static PutRoleRequest ToPutRequest(this GetRoleResponse response)
        {
            var model = MapModel(response);

            return new PutRoleRequest { Data = model, Id = response.Data.Id };
        }

        private static SaveRoleModel MapModel(GetRoleResponse response)
        {
            var model = new SaveRoleModel
                        {
                            Version = response.Data.Version,
                            Name = response.Data.Name,
                            Description = response.Data.Description,
                        };

            return model;
        }
    }
}
