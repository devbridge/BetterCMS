using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Users.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Users.Services
{
    public class DefaultUserService : IUserService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultUserService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultUserService(IRepository repository)
        {
            this.repository = repository;
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
        /// <exception cref="System.NotImplementedException"></exception>
        public System.Collections.Generic.IList<Models.User> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
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
        /// <exception cref="System.NotImplementedException"></exception>
        public System.Collections.Generic.IList<Models.User> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
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
        /// <exception cref="System.NotImplementedException"></exception>
        public System.Collections.Generic.IList<Models.User> GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
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
        /// <exception cref="System.NotImplementedException"></exception>
        public string GetUserNameByEmail(string email)
        {
            return repository
                 .AsQueryable<Models.User>(u => u.Email == email)
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
                 .AsQueryable<Models.User>(u => u.UserName == username)
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
        private System.Collections.Generic.IList<Models.User> GetUsers(int pageIndex, int pageSize, out int totalRecords, Expression<System.Func<Models.User, bool>> expresion = null)
        {
            var query = repository
                 .AsQueryable<Models.User>();

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
    }
}