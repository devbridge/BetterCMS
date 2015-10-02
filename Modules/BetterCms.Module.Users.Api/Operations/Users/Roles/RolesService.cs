using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Users.Roles;
using BetterCms.Module.Api.Operations.Users.Roles.Role;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Users.Api.Operations.Users.Roles
{
    [RoutePrefix("bcms-api")]
    public class RolesController : ApiController, IRolesService
    {
        private readonly IRepository repository;

        private readonly IRoleService roleService;

        public RolesController(IRepository repository, IRoleService roleService)
        {
            this.repository = repository;
            this.roleService = roleService;
        }

        [Route("roles")]
        public GetRolesResponse Get([ModelBinder(typeof(JsonModelBinder))]GetRolesRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var listResponse = repository
                .AsQueryable<Models.Role>()
                .Select(role => new RoleModel
                    {
                        Id = role.Id,
                        Version = role.Version,
                        CreatedBy = role.CreatedByUser,
                        CreatedOn = role.CreatedOn,
                        LastModifiedBy = role.ModifiedByUser,
                        LastModifiedOn = role.ModifiedOn,

                        Name = role.Name,
                        Description = role.Description,
                        IsSystematic = role.IsSystematic
                    })
                .ToDataListResponse(request);

            return new GetRolesResponse { Data = listResponse };
        }

        [Route("roles/")]
        public PostRoleResponse Post(PostRoleRequest request)
        {
            var result = roleService.Put(new PutRoleRequest
                {
                    Data = request.Data,
                    User = request.User
                });

            return new PostRoleResponse { Data = result.Data };
        }
    }
}