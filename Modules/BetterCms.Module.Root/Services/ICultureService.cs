using System.Collections.Generic;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Root.Services
{
    public interface ICultureService
    {
        /// <summary>
        /// Gets the list of cultures.
        /// </summary>
        /// <returns>List of culture lookup values.</returns>
        IEnumerable<LookupKeyValue> GetCultures();

        /// <summary>
        /// Gets the invariant culture model.
        /// </summary>
        /// <returns>Invariant culture model</returns>
        LookupKeyValue GetInvariantCultureModel();
    }
}