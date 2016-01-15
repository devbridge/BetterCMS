// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultTextEncryptor.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using BetterModules.Core.Exceptions;

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
            if (!configuration.Security.EncryptionEnabled)
            {
                return text;
            }

            try
            {
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
            catch (Exception ex)
            {
                throw new CoreException("Encryption failed.", ex);
            }
        }

        public string Decrypt(string encryptedText)
        {
            if (!configuration.Security.EncryptionEnabled)
            {
                return encryptedText;
            }

            try
            {
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
            catch (FormatException)
            {
                return encryptedText;
            }
            catch (Exception ex)
            {
                throw new CoreException("Decryption failed.", ex);
            }
        }

        private byte[] GetBytes()
        {            
            if (keyBytes == null)
            {
                if (configuration.Security.EncryptionEnabled && string.IsNullOrWhiteSpace(configuration.Security.EncryptionKey))
                {
                    throw new CoreException("A ContentEncryptionKey should be provided when an content encryption is enabled (<security enableContentEncryption=\"true\" contentEncryptionKey=\"any key to encrypt data\">)");
                }

                using (var rfc2898 = new Rfc2898DeriveBytes(configuration.Security.EncryptionKey, Encoding.ASCII.GetBytes(Salt)))
                {
                    keyBytes = rfc2898.GetBytes(cryptoProvider.KeySize / 8);
                }
            }

            return keyBytes;
        }
    }
}