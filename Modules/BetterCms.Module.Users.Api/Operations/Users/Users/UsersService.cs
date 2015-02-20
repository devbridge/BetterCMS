using System;
using System.Linq;
using System.Linq.Expressions;

using BetterModules.Core.DataAccess;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;
using BetterCms.Module.Api.Operations.Users.Users;
using BetterCms.Module.Api.Operations.Users.Users.User;
using BetterCms.Module.MediaManager.Services;

using ServiceStack.ServiceInterface;

using PredicateBuilder = ServiceStack.OrmLite.PredicateBuilder;
using UserModel = BetterCms.Module.Api.Operations.Users.Users.UserModel;

namespace BetterCms.Module.Users.Api.Operations.Users.Users
{
    public class UsersService : Service, IUsersService
    {
        private readonly IRepository repository;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        private readonly IUserService userService;

        public UsersService(IRepository repository, IMediaFileUrlResolver fileUrlResolver, IUserService userService)
        {
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
            this.userService = userService;
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

            foreach (var model in listResponse.Items)
            {
                model.ImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.ImageThumbnailUrl);
                model.ImageUrl = fileUrlResolver.EnsureFullPathUrl(model.ImageUrl);
            }

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
                            predicate = PredicateBuilder.Or(predicate, whereClause);
                        }
                        else
                        {
                            predicate = PredicateBuilder.And(predicate, whereClause);
                        }
                    }
                }

                query = query.Where(predicate);
            }

            return query;
        }

        public PostUserResponse Post(PostUserRequest request)
        {
            var result = userService.Put(new PutUserRequest
                {
                    Data = request.Data,
                    User = request.User
                });

            return new PostUserResponse { Data = result.Data };
        }
    }
}