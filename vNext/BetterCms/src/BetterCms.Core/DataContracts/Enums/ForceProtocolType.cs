using System;

namespace BetterCms.Core.DataContracts.Enums
{
    /// <summary>
    /// Indicates page protocol.
    /// </summary>
    [Serializable]
    public enum ForceProtocolType
    {
        /// <summary>
        /// Allow page to be loaded via http or https.
        /// </summary>
        None = 0,

        /// <summary>
        /// Force page loading only via http protocol.
        /// </summary>
        ForceHttp = 1,

        /// <summary>
        /// Force page loading only via https protocol.
        /// </summary>
        ForceHttps = 2
    }
}
