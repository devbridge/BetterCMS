using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;

namespace BetterCms.Core.DataServices
{
    public interface ITagApiService
    {
        IList<ITag> GetTags();

        IQueryable<ITag> GetTagsQueryable();

        IQueryable<TEntity> GetTagsQueryable<TEntity>() 
            where TEntity : Entity, ITag;
    }
}
