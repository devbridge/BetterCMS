using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Operations.Users.Roles.Role;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Users.Api.Operations.Users.Roles.Role
{
    [RoutePrefix("bcms-api")]
    public class RoleController : ApiController, IRoleService
    {
        private readonly IRepository repository;
        
        private readonly IUnitOfWork unitOfWork;

        private readonly Services.IRoleService roleService;

        public RoleController(IRepository repository, Services.IRoleService roleService, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.roleService = roleService;
        }

        [Route("roles/{RoleId}")]
        [Route("roles/by-name/{RoleName}")]
        [ValidationAtttibute]
        public GetRoleResponse Get([ModelBinder(typeof(JsonModelBinder))]GetRoleRequest request)
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
                        Description = role.Description,
                        IsSystematic = role.IsSystematic
                    })
                .FirstOne();

            return new GetRoleResponse { Data = model };
        }

        [Route("roles/{Id}")]
        [UrlPopulator]
        public PutRoleResponse Put(PutRoleRequest request)
        {
            bool isNew;
            unitOfWork.BeginTransaction();
            var role = roleService.SaveRole(request.Id ?? Guid.Empty, request.Data.Version, request.Data.Name, request.Data.Description, out isNew, true);
            unitOfWork.Commit();

            if (isNew)
            {
                Events.UserEvents.Instance.OnRoleCreated(role);
            }
            else
            {
                Events.UserEvents.Instance.OnRoleUpdated(role);
            }

            return new PutRoleResponse { Data = role.Id };
        }

        [Route("roles/{Id}")]
        [UrlPopulator]
        public DeleteRoleResponse Delete(DeleteRoleRequest request)
        {
            unitOfWork.BeginTransaction();
            var role = roleService.DeleteRole(request.Id, request.Data.Version, true);
            unitOfWork.Commit();

            // Notify.
            Events.UserEvents.Instance.OnRoleDeleted(role);

            return new DeleteRoleResponse { Data = true };
        }
    }
}