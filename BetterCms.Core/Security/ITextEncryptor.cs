namespace BetterCms.Core.Security
{
    /// <summary>
    /// Defines interface to encrypt and decrypt text data.
    /// </summary>
    public interface ITextEncryptor
    {
        string Encrypt(string text);

        string Decrypt(string encryptedText);        
    }
}