namespace BetterCms.Core.Environment.Assemblies
{
    /// <summary>
    /// Defines the contract to scan and load assemblies.
    /// </summary>
    public interface IAssemblyManager
    {
        /// <summary>
        /// Loads all available assemblies from working directory.
        /// </summary>
        void AddUploadedModules();

        /// <summary>
        /// Adds referenced modules.
        /// </summary>
        void AddReferencedModules();
    }
}
