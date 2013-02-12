using System.Collections.Generic;

using BetterCms.Api.Models;

namespace BetterCms.Api.Services
{
    public interface ITagApiService
    {
        IList<ITag> GetTags();
    }
}
