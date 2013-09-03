using System;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;
using BetterCms.Module.Api.Operations.Users.Users;

using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Users.Api.Operations.Users.Users
{
    public class UsersService : Service, IUsersService
    {
        private readonly IRepository repository;

        public UsersService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetUsersResponse Get(GetUsersRequest request)
        {
            request.Data.SetDefaultOrder("UserName");

            var query = repository.AsQueryable<Models.User>();
            query = ApplyRolesFilter(query, request.Data);

            var listResponse = query
                .Select(user => new UserModel
                    {
                        Id = user.Id,
                        Version = user.Version,
                        CreatedBy = user.CreatedByUser,
                        CreatedOn = user.CreatedOn,
                        LastModifiedBy = user.ModifiedByUser,
                        LastModifiedOn = user.ModifiedOn,

                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        ImageId = user.Image != null && !user.Image.IsDeleted ? user.Image.Id : (System.Guid?) null,
                        ImageCaption = user.Image != null && !user.Image.IsDeleted ? user.Image.Caption : null,
                        ImageThumbnailUrl = user.Image != null && !user.Image.IsDeleted ? user.Image.PublicThumbnailUrl : null,
                        ImageUrl = user.Image != null && !user.Image.IsDeleted ? user.Image.PublicUrl : null
                    })
                .ToDataListResponse(request);

            return new GetUsersResponse
                       {
                           Data = listResponse
                       };
        }

        /// <summary>
        /// Applies the roles filter.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="model">The model.</param>
        /// <returns>Query with roles filter applied</returns>
        private IQueryable<Models.User> ApplyRolesFilter(IQueryable<Models.User> query, GetUsersModel model)
        {
            if (model.FilterByRoles != null && model.FilterByRoles.Any(role => !string.IsNullOrWhiteSpace(role)))
            {
                var predicate = (model.FilterByRolesConnector == FilterConnector.Or)
                    ? PredicateBuilder.False<Models.User>()
                    : PredicateBuilder.True<Models.User>();

                foreach (var roleName in model.FilterByRoles)
                {
                    if (!string.IsNullOrWhiteSpace(roleName))
                    {
                        Expression<Func<Models.User, bool>> whereClause = user => user.UserRoles.Any(userRole => userRole.Role.Name == roleName && !userRole.Role.IsDeleted);
                        if (model.FilterByRolesConnector == FilterConnector.Or)
                        {
                            predicate = predicate.Or(whereClause);
                        }
                        else
                        {
                            predicate = predicate.And(whereClause);
                        }
                    }
                }

                query = query.Where(predicate);
            }

            return query;
        }
    }
}