using System;
using System.Security.Cryptography;
using System.Text;

namespace BetterCms.Module.Users.Services
{
    public class DefaultAuthenticationService : IAuthenticationService
    {
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

        private byte[] HexToByte(string hexString)
        {
            var bytes = new byte[hexString.Length * sizeof(char)];
            Buffer.BlockCopy(hexString.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}