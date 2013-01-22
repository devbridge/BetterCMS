using System.Collections.Generic;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Blog.Services
{
    public interface IAuthorService
    {
        /// <summary>
        /// Gets the list of author lookup values.
        /// </summary>
        /// <returns>List of author lookup values.</returns>
        IEnumerable<LookupKeyValue> GetAuthors();
    }
}