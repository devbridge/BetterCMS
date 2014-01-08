using System;
using BetterCms.Configuration;

namespace BetterCms
{
    public interface ICmsSearchConfiguration
    {
        string GetValue(string key);
    }
}