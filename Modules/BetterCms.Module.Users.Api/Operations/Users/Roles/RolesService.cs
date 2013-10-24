using System.Linq;

using BetterCms.Core.DataAccess;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Users.Roles;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Users.Api.Operations.Users.Roles
{
    public class RolesService : Service, IRolesService
    {
        private readonly IRepository repository;

        public RolesService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetRolesResponse Get(GetRolesRequest request)
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
                        IsSystematic = role.IsSystematic
                    })
                .ToDataListResponse(request);

            return new GetRolesResponse { Data = listResponse };
        }
    }
}