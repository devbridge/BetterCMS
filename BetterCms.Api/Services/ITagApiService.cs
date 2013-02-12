using System.Collections.Generic;

using BetterCms.Api.Interfaces.Models;

namespace BetterCms.Api.Services
{
    public interface ITagApiService
    {
        // Methods
        IList<ITag> GetTags();
    }
}
