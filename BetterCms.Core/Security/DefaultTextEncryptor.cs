using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using BetterCms.Core.Exceptions;

namespace BetterCms.Core.Security
{
    public class DefaultTextEncryptor : ITextEncryptor
    {
        private const string Salt = "73E1FB42-2383-469F-92B8-53ADF5C8C5C8";

        private readonly SymmetricAlgorithm cryptoProvider;        

        private readonly ICmsConfiguration configuration;

        private byte[] keyBytes;

        public DefaultTextEncryptor(ICmsConfiguration configuration)
        {
            this.configuration = configuration;
            cryptoProvider = new DESCryptoServiceProvider();
        }

        public string Encrypt(string text)
        {         
            if (!configuration.Security.EnableContentEncryption)
            {
                return text;
            }

            var bytes = GetBytes();

            using (var memoryStream = new MemoryStream())
            {
                using (ICryptoTransform encryptor = cryptoProvider.CreateEncryptor(bytes, bytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(text);
                            writer.Flush();
                            cryptoStream.FlushFinalBlock();

                            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                        }
                    }
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            if (!configuration.Security.EnableContentEncryption)
            {
                return encryptedText;
            }

            var bytes = GetBytes();

            using (var memoryStream = new MemoryStream(Convert.FromBase64String(encryptedText)))
            {
                using (ICryptoTransform decryptor = cryptoProvider.CreateDecryptor(bytes, bytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cryptoStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }

        private byte[] GetBytes()
        {            
            if (keyBytes == null)
            {
                if (configuration.Security.EnableContentEncryption && string.IsNullOrWhiteSpace(configuration.Security.ContentEncryptionKey))
                {
                    throw new CmsException("A ContentEncryptionKey should be provided when an content encryption is enabled (<security enableContentEncryption=\"true\" contentEncryptionKey=\"any key to encrypt data\">)");
                }

                using (var rfc2898 = new Rfc2898DeriveBytes(configuration.Security.ContentEncryptionKey, Encoding.ASCII.GetBytes(Salt)))
                {
                    keyBytes = rfc2898.GetBytes(cryptoProvider.KeySize / 8);
                }
            }

            return keyBytes;
        }
    }
}