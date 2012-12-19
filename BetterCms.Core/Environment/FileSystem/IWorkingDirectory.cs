using System.Collections.Generic;
using System.IO;

namespace BetterCms.Core.Environment.FileSystem
{
    /// <summary>
    ///  Defines the contract to manage BetterCMS working directory.
    /// </summary>
    public interface IWorkingDirectory
    {
        /// <summary>
        /// Gets module assembly files from working folder.
        /// </summary>        
        /// <returns>Module assembly file names.</returns>
        IEnumerable<FileInfo> GetAvailableModules();

        /// <summary>
        /// Tries to recopy a module assembly to runtime folder.
        /// </summary>
        /// <param name="module">The module file information.</param>
        /// <returns>Runtime module file information.</returns>
        FileInfo RecopyModulesToRuntimeFolder(FileInfo module);
    }
}
