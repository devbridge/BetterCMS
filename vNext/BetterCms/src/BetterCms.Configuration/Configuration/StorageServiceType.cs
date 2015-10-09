namespace BetterCms.Configuration
{
    public enum StorageServiceType
    {
        /// <summary>
        /// Storage on the local server file system. This is default.
        /// </summary>
        FileSystem = 0,
        
        /// <summary>
        /// Custom storage provider.
        /// </summary>
        Custom = 1,

        /// <summary>
        /// Files are stored using FTP.
        /// </summary>
        Ftp = 2,

        /// <summary>
        ///  Storage service is automatically picked by scanning installed modules for the storage service.
        /// </summary>
        Auto = 3
    }
}