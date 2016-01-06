using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Models;
using BetterCms.Module.Users.ViewModels.User;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;

using NHibernate.Linq;

namespace BetterCms.Module.Users.Services
{
    public class DefaultUserService : IUserService
    {
        private readonly IRepository repository;

        private readonly IUnitOfWork unitOfWork;

        private readonly IAuthenticationService authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultUserService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultUserService(IRepository repository, IAuthenticationService authenticationService, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.authenticationService = authenticationService;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
        /// </summary>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        public IList<User> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return GetUsers(pageIndex, pageSize, out totalRecords, u => u.Email.Contains(emailToMatch));
        }

        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        public IList<User> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return GetUsers(pageIndex, pageSize, out totalRecords, u => u.UserName.Contains(usernameToMatch));
        }

        /// <summary>
        /// Gets a collection of all the users in the data source in pages of data.
        /// </summary>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        public IList<User> GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return GetUsers(pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <param name="email">The e-mail address to search for.</param>
        /// <returns>
        /// The user name associated with the specified e-mail address. If no match is found, return null.
        /// </returns>
        public string GetUserNameByEmail(string email)
        {
            return repository
                 .AsQueryable<User>(u => u.Email == email)
                 .Select(u => u.UserName)
                 .FirstOne();
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// User by username.
        /// </returns>
        public User GetUser(string username)
        {
            return repository
                 .AsQueryable<User>(u => u.UserName == username)
                 .FirstOrDefault();
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <param name="expresion">The expresion.</param>
        /// <returns>
        /// List of users
        /// </returns>
        private IList<User> GetUsers(int pageIndex, int pageSize, out int totalRecords, Expression<System.Func<User, bool>> expresion = null)
        {
            var query = repository
                 .AsQueryable<User>();

            if (expresion != null)
            {
                query = query.Where(expresion);
            }

            var countFuture = query.ToRowCountFutureValue();

            var users = query
                .OrderBy(u => u.UserName)
                .AddPaging((pageIndex - 1) * pageSize + 1, pageSize)
                .ToFuture()
                .ToList();
            totalRecords = countFuture.Value;

            return users;
        }

        /// <summary>
        /// Saves the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="treatNullsAsLists">if set to <c>true</c> treat null lists as empty lists.</param>
        /// <param name="createIfNotExists">if set to <c>true</c> create if not exists.</param>
        /// <returns>
        /// Saved user entity
        /// </returns>
        public User SaveUser(EditUserViewModel model, bool treatNullsAsLists = true, bool createIfNotExists = false)
        {
            ValidateUser(model);

            User user = null;
            var isNew = model.Id.HasDefaultValue();
            if (!isNew)
            {
                user = repository
                    .AsQueryable<User>()
                    .Where(u => u.Id == model.Id)
                    .FetchMany(u => u.UserRoles)
                    .ThenFetch(ur => ur.Role)
                    .ToList()
                    .FirstOrDefault();
                isNew = user == null;

                if (isNew & !createIfNotExists)
                {
                    throw new EntityNotFoundException(typeof(User), model.Id);
                }
            }
            
            if (isNew)
            {
                user = new User { Id = model.Id };

                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    var message = UsersGlobalization.CreateUser_Password_IsRequired;
                    var logMessage = string.Format("{0}, UserName: {1}, Email: {2}", message, model.UserName, model.Email);
                    throw new ValidationException(() => message, logMessage);
                }
            }

            unitOfWork.BeginTransaction();

            if (model.Version > 0)
            {
                user.Version = model.Version;
            }
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var salt = authenticationService.GeneratePasswordSalt();
                user.Password = authenticationService.CreatePasswordHash(model.Password, salt);
                user.Salt = salt;
            }

            if (model.Image != null && model.Image.ImageId.HasValue)
            {
                user.Image = repository.AsProxy<MediaImage>(model.Image.ImageId.Value);
            }
            else
            {
                user.Image = null;
            }

            // Set null list as empty
            if (treatNullsAsLists)
            {
                model.Roles = model.Roles ?? new string[0];
            }

            List<Role> createdRoles = null;
            if (model.Roles != null)
            {
                createdRoles = SaveUserRoles(user, model, createIfNotExists);
            }

            repository.Save(user);
            unitOfWork.Commit();

            // Notify.
            if (createdRoles != null)
            {
                createdRoles.ForEach(Events.UserEvents.Instance.OnRoleCreated);
            }

            if (isNew)
            {
                Events.UserEvents.Instance.OnUserCreated(user);
            }
            else
            {
                Events.UserEvents.Instance.OnUserUpdated(user);
            }

            return user;
        }

        private List<Role> SaveUserRoles(User user, EditUserViewModel request, bool createIfNotExists)
        {
            var dbRoles = user.UserRoles ?? new List<UserRole>();
            var requestRoles = request.Roles ?? new List<string>();
            var createdRoles = new List<Role>();

            // Delete removed roles
            dbRoles
                .Where(dbRole => requestRoles.All(requestRole => requestRole != dbRole.Role.Name))
                .ToList()
                .ForEach(del => repository.Delete(del));

            // Insert new roles
            var rolesToInsert = requestRoles
                .Where(requestRole => dbRoles.All(dbRole => requestRole != dbRole.Role.Name))
                .ToList();

            if (rolesToInsert.Count > 0)
            {
                var roles = repository
                    .AsQueryable<Role>(role => rolesToInsert.Contains(role.Name))
                    .ToList();

                if (createIfNotExists)
                {
                    foreach (var roleName in rolesToInsert.Where(rr => roles.All(r => r.Name != rr)).ToList())
                    {
                        var role = new Role { Name = roleName };
                        createdRoles.Add(role);
                        roles.Add(role);
                        
                        repository.Save(role);
                    }
                }

                rolesToInsert.ForEach(roleName =>
                        repository.Save(new UserRole
                            {
                                User = user,
                                Role = roles.Where(role => role.Name == roleName).FirstOne()
                            }));
            }

            return createdRoles;
        }

        private void ValidateUser(EditUserViewModel model)
        {
            var userName = model.UserName.Trim();
            var existIngId = repository
                .AsQueryable<User>(c => c.UserName == userName && c.Id != model.Id)
                .Select(r => r.Id)
                .FirstOrDefault();

            if (!existIngId.HasDefaultValue())
            {
                var message = string.Format(UsersGlobalization.SaveUse_UserNameExists_Message, model.UserName);
                var logMessage = string.Format("Failed to update user profile. User Name already exists. User Name: {0}, User Email: {1}, Id: {2}", model.UserName, model.Email, model.Id);

                throw new ValidationException(() => message, logMessage);
            }

            var email = model.Email.Trim();
            existIngId = repository
                .AsQueryable<User>(c => c.Email == email && c.Id != model.Id)
                .Select(r => r.Id)
                .FirstOrDefault();

            if (!existIngId.HasDefaultValue())
            {
                var message = string.Format(UsersGlobalization.SaveUse_UserEmailExists_Message, model.Email);
                var logMessage = string.Format("Failed to update user profile. User Email already exists. User Name: {0}, User Email: {1}, Id: {2}", model.UserName, model.Email, model.Id);

                throw new ValidationException(() => message, logMessage);
            }
        }

        public void DeleteUser(Guid id, int version)
        {
            var user = repository.AsQueryable<User>(u => u.Id == id).FirstOne();
            if (version > 0)
            {
                user.Version = version;
            }
            repository.Delete(user);

            if (user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    repository.Delete(userRole);
                }
            }

            unitOfWork.Commit();

            // Notify.
            Events.UserEvents.Instance.OnUserDeleted(user);
        }
    }
}