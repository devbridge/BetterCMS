using System;

namespace BetterCms.Module.Users.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Generates the password salt.
        /// </summary>
        /// <returns>Password Salt encoded in Base64 format</returns>
        string GeneratePasswordSalt();

        /// <summary>
        /// Creates the password hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="passwordSaltBase64">The password salt in Base64.</param>
        /// <returns>Hashed password encoded in Base64 format</returns>
        string CreatePasswordHash(string password, string passwordSaltBase64);

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   <c>true</c>, if username and password are equal.
        /// </returns>
        bool ValidateUser(string username, string password);

        /// <summary>
        /// Gets the user identifier if valid.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>User Id if user is valid.</returns>
        Guid? GetUserIdIfValid(string username, string password);

        /// <summary>
        /// Processes a request to update the password for a membership user.
        /// </summary>
        /// <param name="username">The user to update the password for.</param>
        /// <param name="oldPassword">The current password for the specified user.</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        /// <returns>
        /// true if the password was updated successfully; otherwise, false.
        /// </returns>
        bool ChangePassword(string username, string oldPassword, string newPassword);
    }
}