using System;
using System.Collections.Generic;

using BetterCms.Module.Blog.Models;
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

        void DeleteAuthor(Guid authorId, int version);

        Author CreateAuthor(string name, Guid? imageId, string description);

        Author UpdateAuthor(Guid authorId, int version, string name, Guid? imageId, string description);
    }
}