using System.Linq;

namespace BetterCms.Core.DataAccess.DataContext.Fetching
{
    public interface IFetchRequest<TQueried, TFetch> : IOrderedQueryable<TQueried>
    {
    }
}
