using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataModels;
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

        IQueryable<AuthorModel> GetAuthorsAsQueryable();

        AuthorCreateResponce CreateAuthor(AuthorCreateRequest request);

        AuthorUpdateResponce UpdateAuthor(AuthorUpdateRequest request);

        AuthorDeleteResponce DeleteAuthor(AuthorDeleteRequest request);
    }
}