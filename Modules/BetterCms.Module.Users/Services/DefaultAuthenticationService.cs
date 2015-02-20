using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using BetterModules.Core.DataAccess;

namespace BetterCms.Module.Users.Services
{
    public class DefaultAuthenticationService : IAuthenticationService
    {
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAuthenticationService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultAuthenticationService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Generates the password salt.
        /// </summary>
        /// <returns>
        /// Password Salt encoded in Base64 format
        /// </returns>
        /// <exception cref="System.SystemException">Failed to generate password salt.</exception>
        public string GeneratePasswordSalt()
        {
            try
            {
                var rng = new RNGCryptoServiceProvider();
                var buff = new byte[8];
                rng.GetBytes(buff);
                return Convert.ToBase64String(buff);
            }
            catch (Exception ex)
            {
                 throw new SystemException("Failed to generate password salt.", ex);
            }
        }

        /// <summary>
        /// Creates the password hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="passwordSaltBase64">The password salt in Base64.</param>
        /// <returns>
        /// Hashed password encoded in Base64 format
        /// </returns>
        /// <exception cref="System.SystemException">Failed to create password hash.</exception>
        public string CreatePasswordHash(string password, string passwordSaltBase64)
        {
            try
            {
                var hash = new HMACSHA1 { Key = HexToByte(passwordSaltBase64) };
                var encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                return encodedPassword;
            }
            catch (Exception ex)
            {
                throw new SystemException("Failed to create password hash.", ex);
            }
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   <c>true</c>, if username and password are equal.
        /// </returns>
        public bool ValidateUser(string username, string password)
        {
            var userId = GetUserIdIfValid(username, password);
            return userId.HasValue && userId.Value != Guid.Empty;
        }

        /// <summary>
        /// Gets the user identifier if valid.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// User Id if user is valid.
        /// </returns>
        public Guid? GetUserIdIfValid(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var user = repository
               .AsQueryable<Models.User>(u => u.UserName == username)
                   .Select(u => new
                   {
                       Id = u.Id,
                       Salt = u.Salt,
                       Password = u.Password
                   })
               .FirstOrDefault();

            if (user != null)
            {
                if (CheckPassword(password, user.Password, user.Salt))
                {
                    return user.Id;
                }
            }

            return null;
        }

        /// <summary>
        /// Processes a request to update the password for a membership user.
        /// </summary>
        /// <param name="username">The user to update the password for.</param>
        /// <param name="oldPassword">The current password for the specified user.</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        /// <returns>
        /// true if the password was updated successfully; otherwise, false.
        /// </returns>
        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || !ValidateUser(username, oldPassword))
            {
                return false;
            }

            var salt = GeneratePasswordSalt();
            newPassword = CreatePasswordHash(newPassword, salt);

            var user = repository.First<Models.User>(u => u.UserName == username);
            user.Salt = salt;
            user.Password = newPassword;

            repository.Save(user);

            return true;
        }

        /// <summary>
        /// Converts HEX string to bytes array.
        /// </summary>
        /// <param name="hexString">The hex string.</param>
        /// <returns>Bytes array.</returns>
        private static byte[] HexToByte(string hexString)
        {
            var bytes = new byte[hexString.Length * sizeof(char)];
            Buffer.BlockCopy(hexString.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Checks the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="dbpassword">The dbpassword.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>
        ///   <c>true</c>, if passwords are equal
        /// </returns>
        private bool CheckPassword(string password, string dbpassword, string salt)
        {
            string pass1 = CreatePasswordHash(password, salt);
            string pass2 = dbpassword;

            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }
    }
}