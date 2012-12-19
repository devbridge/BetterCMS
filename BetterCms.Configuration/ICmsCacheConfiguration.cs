using System;

using BetterCms.Configuration;

namespace BetterCms
{
    public interface ICmsCacheConfiguration
    {
        bool Enabled { get; }

        TimeSpan Timeout { get; }

        CacheServiceType CacheType { get; }

        string GetValue(string key);
    }
}