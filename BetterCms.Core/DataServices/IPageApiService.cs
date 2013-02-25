using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;

namespace BetterCms.Core.DataServices
{
    public interface IPageApiService
    {
        IList<IPage> GetPages();

        IQueryable<IPage> GetPagesQueryable();

        IQueryable<TEntity> GetPagesQueryable<TEntity>()
            where TEntity : Entity, IPage;
    }
}
