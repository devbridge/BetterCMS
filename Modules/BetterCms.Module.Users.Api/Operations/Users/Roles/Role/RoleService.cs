using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Operations.Users.Roles.Role;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Users.Api.Operations.Users.Roles.Role
{
    public class RoleService : Service, IRoleService
    {
        private readonly IRepository repository;

        public RoleService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetRoleResponse Get(GetRoleRequest request)
        {
            var query = repository
                .AsQueryable<Models.Role>();

            if (request.RoleId.HasValue)
            {
                query = query.Where(role => role.Id == request.RoleId);
            }
            else
            {
                query = query.Where(role => role.Name == request.RoleName);
            }
            
            var model = query
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
                .FirstOne();

            return new GetRoleResponse { Data = model };
        }
    }
}