using System;
using BetterCms.Configuration;

namespace BetterCms
{
    public interface ICmsStorageConfiguration
    {
        string PublicContentUrlRoot { get; set; }

        string ContentRoot { get; set; }

        StorageServiceType ServiceType { get; set; }

        TimeSpan ProcessTimeout { get; set; }

        string GetValue(string key);
    }
}