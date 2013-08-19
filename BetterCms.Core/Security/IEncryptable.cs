namespace BetterCms.Core.Security
{
    public interface IEncryptable
    {
        string Salt { get; set; }
    }
}
